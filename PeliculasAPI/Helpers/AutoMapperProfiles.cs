using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite.Geometries;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<Generos, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Generos>();

            CreateMap<Review, ReviewDTO>()
                .ForMember(x => x.NombreUsuario, x => x.MapFrom(y => y.Usuario.UserName));

            CreateMap<ReviewDTO, Review>();
            CreateMap<ReviewCreactionDTO, Review>();

            CreateMap<IdentityUser, UsuarioDTO>().ReverseMap();

            // Mapear desde Point hasta Longitud y Latitud

            CreateMap<SalaDeCine, SalaDeCineDTO>()
                .ForMember(x => x.Latitud, x => x.MapFrom(y => y.Ubicacion.Y))
                .ForMember(x => x.Longitud, x => x.MapFrom(y => y.Ubicacion.X));

            

            // Mapear desde longitud y latitud hasta point
            CreateMap<SalaDeCineDTO, SalaDeCine>()
                .ForMember(x => x.Ubicacion, x => x.MapFrom(y =>
                 geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud))));

            CreateMap<SalaDeCineCreacionDTO, SalaDeCine>()
               .ForMember(x => x.Ubicacion, x => x.MapFrom(y =>
                geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud))));


            CreateMap<SalaDeCineCreacionDTO, SalaDeCine>();

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>().ForMember(x => x.Foto, options => options.Ignore());
            CreateMap<ActorPatchDTO, Actor>().ReverseMap();


            CreateMap<Pelicula, PeliculaDTO>().ReverseMap();
            CreateMap<PeliculaCreacionDTO, Pelicula>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.PeliculasGeneros, opt => opt.MapFrom(MapPeliculasGeneros))
                .ForMember(x => x.PeliculasActores, opt => opt.MapFrom(MapPeliculasActores));

            CreateMap<Pelicula, PeliculaDetallesDTO>()
                .ForMember(x => x.Generos, opt => opt.MapFrom(MapPeliculasGeneros))
                .ForMember(x => x.Actores, opt => opt.MapFrom(MapPeliculasActores));
            CreateMap<PeliculaPatchDTO, Pelicula>().ReverseMap();
        }

        private List<ActoresPeliculaDetalleDTO> MapPeliculasActores(Pelicula pelicula, PeliculaDetallesDTO peliculaDetallesDTO)
        {
            var resultado = new List<ActoresPeliculaDetalleDTO>();
            if(pelicula.PeliculasActores == null) { return resultado; }
            foreach (var actorPelicula in pelicula.PeliculasActores)
            {
                resultado.Add(new ActoresPeliculaDetalleDTO
                {
                    ActorId = actorPelicula.ActorId,
                    Personaje = actorPelicula.Personaje,
                    NombrePersona = actorPelicula.Actor.Nombre
                });
            }
            return resultado;
        }

        private List<GeneroDTO> MapPeliculasGeneros(Pelicula pelicula, PeliculaDetallesDTO peliculaDetallesDTO)
        {
            var resultado = new List<GeneroDTO>();
            if(pelicula.PeliculasGeneros == null) { return resultado; }
            foreach(var generoPelicula in pelicula.PeliculasGeneros)
            {
                resultado.Add(new GeneroDTO() { Id = generoPelicula.GeneroId, Nombre = generoPelicula.Genero.Nombre });
            }
            return resultado;
        }


        private List<PeliculasGeneros> MapPeliculasGeneros(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            var resultado = new List<PeliculasGeneros>();
            if(peliculaCreacionDTO.GenerosIDs == null) { return resultado; }
            foreach(var id in peliculaCreacionDTO.GenerosIDs)
            {
                resultado.Add(new PeliculasGeneros() { GeneroId = id });
            }
            return resultado;
        }

        private List<PeliculasActores> MapPeliculasActores(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            var resultado = new List<PeliculasActores>();
            if (peliculaCreacionDTO.Actores == null) { return resultado; }
            foreach(var actor in peliculaCreacionDTO.Actores)
            {
                resultado.Add(new PeliculasActores()
                {
                    ActorId = actor.ActorId, Personaje = actor.Personaje
                });
            }
            return resultado;
        }
    }
}
