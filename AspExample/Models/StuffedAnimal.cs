using System.Text.Json.Serialization;

namespace AspExample.Models
{
   public class StuffedAnimal
   {
      //Automatically the primary key because of its name. You can also use "ToyboxId", and other variations
      public int Id {get;set;}

      public string Name {get;set;}
      public double Stuffiness {get;set;}

      //This further helps solidify how the relationship works between StuffedAnimal and Toybox (along with
      //the actual object below). Plus, we JsonIgnore the actual parent field below to avoid circular serialization, 
      //so we need SOMETHING to identify the parent
      public int ToyboxId {get;set;}

      //The parent of this StuffedAnimal. EntityFramework automatically knows this because of the type,
      //and to some extent the name. 
      [JsonIgnore]
      public Toybox Toybox {get;set;}

   }
}