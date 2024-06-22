using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthKalumManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthKalumManagement.DbContext
{
    public class AuthKalumManagementContext : IdentityDbContext <ApplicationUser>//esta clase servira para la conexion  a sia la base de datos 
    {
        public AuthKalumManagementContext(DbContextOptions<AuthKalumManagementContext> options) : base(options)
        {

        }
    }
}