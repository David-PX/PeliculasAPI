using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.DTOs
{
    public class ReviewCreactionDTO
    {
        public string Comentario { get; set; } 
        [Range(1,5)]
        public int Puntuacion { get; set; }
    }
}
