using MyKoloApi.Data;
using MyKoloApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyKoloApi.Controllers
{


    public interface IClaimsManager
    {
        bool SaveClaimsToDatabase(List<Claim> claims, string userId);
    }

    public class ClaimsManager:IClaimsManager
    {
        private readonly AppDbContext _context;
        public ClaimsManager(AppDbContext context)
        {
            _context = context;
        }


        public  bool SaveClaimsToDatabase(List<Claim> claims,string userId)
        {
            try
            {
                foreach (var claim in claims)
                {
                    UserClaim userClaim = new UserClaim()
                    {
                       
                        UserId = userId,
                        Name = claim.Type,
                        Value = claim.Value
                    };

                    _context.Claims.Add(userClaim);
                }
                _context.SaveChanges();
                return true;
                
            }
            catch (Exception e)
            {
                // email this to dev or Log this using a logger framework
                Console.WriteLine(e);
                return false;

            }
            
        }





    }
}
