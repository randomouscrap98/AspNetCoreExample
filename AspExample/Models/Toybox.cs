using System.Collections.Generic;
using System.Drawing;

namespace AspExample.Models
{
   public class Toybox
   {
      //Automatically the primary key because of its name. You can also use "ToyboxId", and other variations
      public int Id {get;set;}

      public int BoxSize {get;set;}

      //Yes, you can even use enums, they are converted to database types automatically
      public ConstructionMaterial Material {get;set;}

      //Ahh but this one is special! We're going to convert this one manually in the dbcontext
      //because we can't "really" store a "Color" in a database, but you don't have to worry about
      //that in your model. You want to keep as much logic as possible out of your models
      public Color Color {get;set;}

      //The StuffedAnimals inside this box, the "children" of this entity. EntityFramework automatically
      //knows this represents the children because of the type and to some extent the name. Stick to standard
      //naming EntityFramework naming conventions, as it uses the names to understand most of the context
      public List<StuffedAnimal> StuffedAnimals {get;set;} = new List<StuffedAnimal>();
   }
}