using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Entidades
{
    public interface IId 
    {
        public int Id { get; set; }
    }
}

// This interface is for the firstOrDefault Method in a CustomBaseController
