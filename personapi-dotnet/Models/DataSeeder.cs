using personapi_dotnet.Models.Entities;
using System;
using System.Collections.Generic; // Use List
using System.Linq;

namespace personapi_dotnet.Models
{
    public static class DataSeeder
    {
        public static void SeedData(PersonaDbContext context)
        {
            // Use Any() on a specific set to check for seeding
            if (context.Personas.Any(p => p.Cc == 12345678)) // Check if Juan Perez exists
            {
                return; // DB has likely been seeded
            }

            // --- Seed Profesiones ---
            var profesionIngeniero = new Profesion { Id = 1, Nom = "Ingeniero de Software", Des = "Desarrolla y mantiene aplicaciones de software." };
            var profesionDisenador = new Profesion { Id = 2, Nom = "Diseñador Grafico", Des = "Crea conceptos visuales para comunicar ideas." };
            var profesionAbogado = new Profesion { Id = 3, Nom = "Abogado", Des = "Asesora y representa a clientes en asuntos legales." };
            var profesionMedico = new Profesion { Id = 4, Nom = "Médico General", Des = "Diagnostica y trata enfermedades comunes." };
            var profesionContador = new Profesion { Id = 5, Nom = "Contador Público", Des = "Gestiona registros financieros." };

            context.Profesions.AddRange(profesionIngeniero, profesionDisenador, profesionAbogado, profesionMedico, profesionContador);

            // --- Seed Personas ---
            var personaJuan = new Persona { Nombre = "Juan", Apellido = "Perez", Edad = 30, Cc = 12345678, Genero = "M" };
            var personaMaria = new Persona { Nombre = "Maria", Apellido = "Lopez", Edad = 25, Cc = 87654321, Genero = "F" };
            var personaCarlos = new Persona { Nombre = "Carlos", Apellido = "Gomez", Edad = 42, Cc = 11223344, Genero = "M" };
            var personaAna = new Persona { Nombre = "Ana", Apellido = "Martinez", Edad = 28, Cc = 55667788, Genero = "F" };
            var personaLuis = new Persona { Nombre = "Luis", Apellido = "Rodriguez", Edad = 55, Cc = 99001122, Genero = "M" };

            context.Personas.AddRange(personaJuan, personaMaria, personaCarlos, personaAna, personaLuis);

            // --- Seed Telefonos ---
            context.Telefonos.AddRange(
                new Telefono { Num = "3101234567", Oper = "Claro", Duenio = personaJuan.Cc },
                new Telefono { Num = "3209876543", Oper = "Movistar", Duenio = personaMaria.Cc },
                new Telefono { Num = "3005551212", Oper = "Tigo", Duenio = personaCarlos.Cc },
                new Telefono { Num = "3117654321", Oper = "Claro", Duenio = personaJuan.Cc }, // Juan's second phone
                new Telefono { Num = "3151112233", Oper = "WOM", Duenio = personaAna.Cc },
                new Telefono { Num = "3014445566", Oper = "Movistar", Duenio = personaLuis.Cc },
                new Telefono { Num = "3187778899", Oper = "Claro", Duenio = personaAna.Cc }  // Ana's second phone
            );

            // --- Seed Estudios ---
            context.Estudios.AddRange(
                new Estudio { Fecha = new DateOnly(2018, 12, 15), Univer = "Universidad Nacional", IdProf = profesionIngeniero.Id, CcPer = personaJuan.Cc },
                new Estudio { Fecha = new DateOnly(2020, 6, 30), Univer = "Universidad de los Andes", IdProf = profesionDisenador.Id, CcPer = personaMaria.Cc },
                new Estudio { Fecha = new DateOnly(2005, 3, 10), Univer = "Universidad Javeriana", IdProf = profesionAbogado.Id, CcPer = personaCarlos.Cc },
                new Estudio { Fecha = new DateOnly(2010, 8, 20), Univer = "Universidad Externado", IdProf = profesionAbogado.Id, CcPer = personaJuan.Cc }, // Juan also studied law
                new Estudio { Fecha = new DateOnly(2019, 5, 25), Univer = "Universidad del Valle", IdProf = profesionMedico.Id, CcPer = personaAna.Cc },
                new Estudio { Fecha = new DateOnly(1990, 11, 30), Univer = "Universidad Pontificia Bolivariana", IdProf = profesionContador.Id, CcPer = personaLuis.Cc },
                new Estudio { Fecha = new DateOnly(2022, 1, 15), Univer = "Universidad EAFIT", IdProf = profesionIngeniero.Id, CcPer = personaAna.Cc } // Ana also studied engineering
            );

            context.SaveChanges(); // Save all changes at once
        }
    }
} 