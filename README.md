# DiabloInterfaceAPI
API for making requests to [DiabloInterface](https://github.com/Zutatensuppe/DiabloInterface).

## Example
Get name of the armor the player has equipped.
```C#
using (var client = new DiabloInterfaceConnection("DiabloInterfaceItems"))
{
    var request = new ItemRequest(ItemRequest.Slot.Armor);
    var response = client.Request(request);
    
    if (!response.IsValid)
    {
        Console.WriteLine("Invalid request.");
    }
    else if (!response.Success)
    {
        Console.WriteLine("No armor equipped.");
    }
    else
    {
        Console.WriteLine("Item name: {0}", response.Items[0].ItemName);
    }
}
```
