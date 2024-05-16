using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Faux.Lib.Exceptions;
using Faux.Lib.Generators;
using Faux.Lib.Models;

namespace Faux.Lib;

public class Faux<T>(T model, FauxOptions? options = null) : IFaux
    where T : new()
{
    private T Model { get; } = model;
    private readonly FauxOptions? _options = options ?? new FauxOptions();
    private readonly List<Withs> _withs = [];
    private readonly List<Sets> _sets = [];
    private readonly List<T> _generated = [];

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };
    
    public Faux<T> With<TR>(Expression<Func<T, TR>> property, Delegate func, params object[] args)
    {

         ArgumentNullException.ThrowIfNull(nameof(property));
         ArgumentNullException.ThrowIfNull(func);
         
         var propertyInfo = ((MemberExpression)property.Body).Member as PropertyInfo;
         
         ArgumentNullException.ThrowIfNull(propertyInfo);
         ArgumentException.ThrowIfNullOrWhiteSpace(propertyInfo.Name);

         if (_sets.FirstOrDefault(v => v.PropertyName == propertyInfo.Name && v.PropertyId == propertyInfo.MetadataToken) != null)
         {
             throw new DuplicateStatementException(
                 $"Cannot declare a with statement for property: {property.Name} due to it already being set");
         }

         _withs.Add(new Withs
         {
             PropertyName = propertyInfo.Name,
             PropertyId = propertyInfo.MetadataToken,
             Function = func,
             Args = args
         });
         return this;
    }

    public Faux<T> Set<TR>(Expression<Func<T, TR>> property, TR val)
    {
        ArgumentNullException.ThrowIfNull(property);
        ArgumentNullException.ThrowIfNull(val);
        
        var propertyInfo = ((MemberExpression)property.Body).Member as PropertyInfo;
        
        ArgumentNullException.ThrowIfNull(propertyInfo);
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyInfo.Name);

        if (_withs.FirstOrDefault(
                v => v.PropertyName == propertyInfo.Name && v.PropertyId == propertyInfo.MetadataToken) != null)
        {
            throw new DuplicateStatementException(
                             $"Cannot declare a with statement for property: {property.Name} due to it already being set");
        }
        
        _sets.Add(new Sets
        {
            PropertyName = propertyInfo.Name,
            PropertyId = propertyInfo.MetadataToken,
            Value = val
        });
        return this;
    }
    
    
    public TVal GenerateSingle<TVal>(TVal val) where TVal : new()
    {
            var localModel = val?.GetType().GetProperties().Length == 0 || val == null ? new  TVal() : val;
            var props = localModel.GetType().GetProperties();
            foreach (var prop in props)
            {
                if (!prop.CanWrite) continue;
                var setVal =
                    _sets.FirstOrDefault(s => s.PropertyId == prop.MetadataToken && s.PropertyName == prop.Name);
                if (setVal != null)
                {
                    prop.SetValue(localModel, setVal.Value, null);
                }
                else
                {
                    var withs = _withs.FirstOrDefault(v =>
                        v.PropertyName == prop.Name && v.PropertyId == prop.MetadataToken);
                    if (withs == null)
                    {
                        GenerateRandomValues(localModel, prop);
                    }
                    else
                    {
                        var res = withs.Args.Length > 0
                            ? withs.Function.DynamicInvoke(withs.Args)
                            : withs.Function.DynamicInvoke();
                        prop.SetValue(localModel, res, null);
                    }
                }
            }
            return localModel;
    }

    public IList<T> GenerateList(int count = 1)
    {
        for (var i = 0; i < count; i++)
        {
            _generated.Add(GenerateSingle(Model));
        }
        return _generated;
    }

    public Faux<T> GenerateMultiple(int count = 1)
    {
        _ = GenerateList(count);
        return this;
    }

    public string ToJson() => JsonSerializer.Serialize(_generated, _jsonSerializerOptions);


    public IEnumerable<T> GetGenerated() => _generated;


    private void GenerateRandomValues<TVal>(TVal model, PropertyInfo prop)
    {
        var generated = GenerateFromType(prop.PropertyType);
        prop.SetValue(model, generated);
    }

    private object? GenerateFromType(Type type)
    {
        switch (Type.GetTypeCode(type))
        {
            case TypeCode.String:
                return StringGenerator.Generate(_options!.StringLength);
            case TypeCode.Empty:
                break;
            case TypeCode.Object:
                if (typeof(IDictionary).IsAssignableFrom(type))
                {
                    var coll = Activator.CreateInstance(type);
                    var add = GetAddMethod(type, 2);
                    var key = type.GetGenericArguments().FirstOrDefault();
                    var val = type.GetGenericArguments()[1];
                    if (key == null) return coll;
                    {
                        for (var i = 0; i < _options!.InnerListLength; i++)
                        {
                            var k = GenerateFromType(key);
                            var v = GenerateFromType(val);
                            add.Invoke(coll, [k, v]);
                        }
                    }
                    return coll;
                }
                
                if (!typeof(IEnumerable).IsAssignableFrom(type)) return GenerateSingle(Activator.CreateInstance(type));
                {
                    var coll = Activator.CreateInstance(type);
                    var add = GetAddMethod(type, 1);
                    var genType = type.GetGenericArguments().FirstOrDefault();
                    for (var i = 0; i < _options!.InnerListLength; i++)
                    { 
                        if(genType != null)
                            add.Invoke(coll, [GenerateFromType(genType)]);
                    }

                    return coll;
                }
            case TypeCode.DBNull:
                break;
            case TypeCode.Boolean:
                return BoolGenerator.Generate();
            case TypeCode.Char:
                break;
            case TypeCode.SByte:
                return NumberGenerator.GenerateSByte();
            case TypeCode.Byte:
                return NumberGenerator.GenerateByte();
            case TypeCode.Int16:
                return NumberGenerator.GenerateShort();
            case TypeCode.UInt16:
                return NumberGenerator.GenerateUShort();
            case TypeCode.Int32:
                return NumberGenerator.GenerateInt(_options!.IntRange[0], _options!.IntRange[1]);
            case TypeCode.UInt32:
                return NumberGenerator.GenerateUInt();
            case TypeCode.Int64:
                return NumberGenerator.GenerateLong();
            case TypeCode.UInt64:
                return NumberGenerator.GenerateLong();
            case TypeCode.Single:
                return NumberGenerator.GenerateSingle();
            case TypeCode.Double:
                return NumberGenerator.GenerateDouble();
            case TypeCode.Decimal:
                return DecimalGenerator.Generate();
            case TypeCode.DateTime:
                return DateTimeGenerator.Generate(DateTime.MinValue, DateTime.MaxValue);
            default:
                throw new ArgumentOutOfRangeException($"Unknown type code of {Type.GetTypeCode(type)}");
        }

        return null;
    }

    private static MethodInfo GetAddMethod(Type type, int paramCount)
    {
        var methods = type.GetMethods();
        var name = type.Name[..type.Name.IndexOf('`')];
        return name switch
        {
            "Queue" => GetMethod(methods, "Enqueue", paramCount),
            "ConcurrentQueue" => GetMethod(methods, "Enqueue", paramCount),
            "Stack" => GetMethod(methods, "Push", paramCount),
            "ConcurrentStack" => GetMethod(methods, "Enqueue", paramCount),
            _ => GetMethod(methods, "Add", paramCount)
        };
    }

    private static MethodInfo GetMethod(MethodInfo[] methods, string method, int paramCount = 1)
    {
        ArgumentNullException.ThrowIfNull(methods);
        ArgumentException.ThrowIfNullOrWhiteSpace(method);
        
        if (paramCount <= 0)
            throw new InvalidOperationException(nameof(paramCount));
        
        return methods.FirstOrDefault(n => n.Name == method && n.GetParameters().Length == paramCount)!;
    }
}