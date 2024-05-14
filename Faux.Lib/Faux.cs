using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using Faux.Lib.Generators;
using Faux.Lib.Models;

namespace Faux.Lib;

public class Faux<T>(T model) : IFaux
    where T : new()
{
    public T Model { get; private set; } = model;
    private int _stringLength = 10;
    private readonly List<Withs> _withs = [];
    private int _innerListAmount = 5;
    
    public Faux<T> StringLength(int length)
    {
        _stringLength = length;
        return this;
    }
    
    public Faux<T> With<R>(Expression<Func<T, R>> property, Delegate func, params object[] args)
    {
        var propertyInfo = ((MemberExpression)property.Body).Member as PropertyInfo;
        _withs.Add(new Withs
        {
            PropertyName = propertyInfo.Name,
            Function = func,
            Args = args
        });
        return this;
    }
    
    public TVal GenerateSingle<TVal>(TVal val) where TVal : new()
    {
            var localModel = val.GetType().GetProperties().Length == 0 ? new  TVal() : val;
            var props = localModel.GetType().GetProperties();
            foreach (var prop in props)
            {
                if (!prop.CanWrite) continue;
                var withsVal = _withs.FirstOrDefault(v => v.PropertyName == prop.Name);
                if (withsVal == null)
                {
                    GenerateRandomValues(localModel, prop);
                }
                else
                {
                    var res = withsVal.Args.Length > 0
                        ? withsVal.Function.DynamicInvoke(withsVal.Args)
                        : withsVal.Function.DynamicInvoke();
                    prop.SetValue(localModel, res, null);
                }
            }

            return localModel;
    }

    public IEnumerable<T> GenerateMultiple(int count = 1)
    {
        var generated = new List<T>();
        for (var i = 0; i < count; i++)
        {
            generated.Add(GenerateSingle(Model));
        }

        return generated;
    }


    private void GenerateRandomValues<T>(T model, PropertyInfo prop = null)
    {
        var generated = GenerateFromType(prop.PropertyType);
        prop.SetValue(model, generated);
    }

    private object GenerateFromType(Type type)
    {
        switch (Type.GetTypeCode(type))
        {
            case TypeCode.String:
                return StringGenerator.Generate(_stringLength);
            case TypeCode.Empty:
                break;
            case TypeCode.Object:
                if (typeof(IDictionary).IsAssignableFrom(type))
                {
                    var coll = Activator.CreateInstance(type);
                    var add = GetAddMethod(type, 2);
                    var key = type.GetGenericArguments().FirstOrDefault(k => k != null);
                    var val = type.GetGenericArguments()[1];
                    if (key == null || val == null) return coll;
                    {
                        for (var i = 0; i < _innerListAmount; i++)
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
                var genType = type.GetGenericArguments().FirstOrDefault(v => v != null);
                for (var i = 0; i < _innerListAmount; i++)
                { 
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
                return NumberGenerator.GenerateInt();
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
                throw new ArgumentOutOfRangeException();
        }

        return null;
    }

    private MethodInfo GetAddMethod(Type type, int paramCount) => type.GetMethods()
        .FirstOrDefault(n => n.Name == "Add" && n.GetParameters().Length == paramCount);
}