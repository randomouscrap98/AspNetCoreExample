using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using AspExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AspExample.Controllers
{
   //A configuration for the controller, this is a decent pattern to have. This way, you can add a singleton for
   //each config type to your IServiceCollection, and they can come from configuration files and such. Ours comes from
   //code, but it's just to show an example; this controller and config here don't need to know where the configuration
   //values come from, they just get passed in to the constructor
   public class ToyboxControllerConfig
   {
      public List<string> RandomNames {get;set;}
   }
   
   [ApiController] //This sets up standard controller stuff
   [Route("[controller]")] //This says how to get to this controller in a URL, in this case, it is URLBASE/toybox (an auto-generated name)
   public class ToyboxController : ControllerBase
   {
      private readonly ILogger logger;
      private MyDbContext dbContext;
      private Random random;
      private ToyboxControllerConfig config;

      //The order of the parameters and all that doesn't matter, because the IServiceCollection we set up
      //in "Startup.cs" can automatically fill in the parameters so long as they have definitions for all the 
      //types. We didn't need to add an "ILogger" because the logger
      public ToyboxController(ILogger<ToyboxController> logger, MyDbContext dbcontext, ToyboxControllerConfig config)
      {
         this.logger = logger;
         this.dbContext = dbcontext;
         this.config = config;
         random = new Random();
      }

      [HttpGet("{toyboxid}")] //this is further routing to say how to get to this particular endpoint in the controller.
                              //in this case, it's URLBASE/toybox/5 (or any other id)
      public async Task<ActionResult<Toybox>> GetToybox([FromRoute]int toyboxid)
      {
         //Look up the toybox by id. This is VERY efficient, the sql generated does NOT iterate over everything!
         //NOTE: by default, the related entities are NOT included! You CAN ask the dbcontext to always include related entities,
         //but it's better to just leave it off by default. If you ACCESS the field, the related entities are obtained at that 
         //point. Since we're NOT modifying or querying the StuffedAnimals property, we NEED to forcibly include it.
         var toybox = await dbContext.Toyboxes.Include(x => x.StuffedAnimals).FirstOrDefaultAsync(x => x.Id == toyboxid);

         if(toybox == null)
            return NotFound($"Can't find toybox with id {toyboxid}"); //This returns a 404 status code
         
         return toybox;
      }

      //These SHOULD be "post", but it's too much effort to test that way
      [HttpGet("makenewtoybox")]
      public async Task<ActionResult<Toybox>> MakeNewToybox()
      {
         //To make database objects, you just... make them. Leave ID blank to tell entity framework this is a "new" entity
         var box = new Toybox()
         {
            Color = Color.AliceBlue,
            BoxSize = 5,
            Material = ConstructionMaterial.Wood
         };

         //AND you can just immediately add stuffed animals! You don't have to do it like this ofc, but make sure
         //you set the parent on all of them!
         box.StuffedAnimals.Add(new StuffedAnimal()
         {
            Name = config.RandomNames[random.Next() % config.RandomNames.Count],
            Stuffiness = DateTime.Now.Ticks / 1000000,
            Toybox = box
         });

         //This is how you add new objects. To instead update them, just look them up from the database, make the 
         //changes, then call "SaveChangesAsync". The objects are tracked automatically. If you feel like you don't
         //trust entity framework to track entities from the database, you can call "Update" like this "Add" here on
         //the object you modified, then still call SaveChangesAsync
         dbContext.Add(box);
         await dbContext.SaveChangesAsync();

         //After this, the toybox will be "tracked" and have an id assigned to it! Same for all the stuffed animals!
         return box;
      }

      [HttpGet("makenewanimal/{toyboxid}")]
      public async Task<ActionResult<StuffedAnimal>> MakeNewAnimal([FromRoute]int toyboxid)
      {
         //Look up the toybox by id. This is VERY efficient, the sql generated does NOT iterate over everything!
         var toybox = await dbContext.Toyboxes.FindAsync(toyboxid);
         //var toybox = await dbContext.Toyboxes.Include(x => x.StuffedAnimals).FirstOrDefaultAsync(x => x.Id == toyboxid);

         if(toybox == null)
            return NotFound($"Can't find toybox with id {toyboxid}"); //This returns a 404 status code

         //AND you can just immediately add stuffed animals! You don't have to do it like this ofc, but make sure
         //you set the parent on all of them!
         var animal = new StuffedAnimal()
         {
            Name = config.RandomNames[random.Next() % config.RandomNames.Count],
            Stuffiness = DateTime.Now.Ticks / 1000000,
            Toybox = toybox
         };
         toybox.StuffedAnimals.Add(animal);

         //We're being pedantic by calling update, toybox is already a tracked object. No harm though
         dbContext.Update(toybox);
         await dbContext.SaveChangesAsync();

         //After this, the animal will ALSO be "tracked" and have an id assigned to it!
         return animal;
      }
   }
}
