using Newtonsoft.Json;
namespace CrazyRisk.Shared.Networking;
public enum DataCategory
{
    Control,
    Action,
    Claim
}

public interface IDataObject
{   
    DataCategory Category {get;}
    string WrapDataObject();
}


// This class is used so that any DataObject handles their own serialization
public abstract class BaseDataObject : IDataObject
{   
    public abstract DataCategory Category { get; }
    public virtual string WrapDataObject()
    {
        return JsonConvert.SerializeObject(
            this,
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }
        );
    }
}


public static class DataObjectHandler
{
    public static IDataObject UnwrapObject(string json)
    {
        var result = JsonConvert.DeserializeObject<IDataObject>(
            json,
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }
        );

        if (result is null)
            throw new InvalidOperationException("Deserialization Failure!");

        return result;
    }
}


