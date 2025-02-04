﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;// Para usar la fecha en formato regional

namespace Cars
{
    class CarCRUD
    {

        public Configuraciones config; // path
        public CarCRUD() // CONSTRUCTOR para inicializar las variables
        {
            // Empienza -> C:\Users\User\Desktop\MakingSense\Cars\Cars\bin\Debug\net5.0
            string jsonConfigs = File.ReadAllText(@"../../../appsettings.json");
            config = JsonSerializer.Deserialize<Configuraciones>(jsonConfigs);
        }

        public List<Car> LeerListaCars()
        {        
            string jsonString = File.ReadAllText(config.path);
            List<Car> listaCars = JsonSerializer.Deserialize<List<Car>>(jsonString);
            return listaCars;
        }

        public void ActualizarJSON(List<Car> cars)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };// Opcion con IDENTACION
            string CarsSerializado = JsonSerializer.Serialize(cars, options);
            File.WriteAllText(config.path, CarsSerializado);
        }

        public int IndicePorID(int id)
        {
            List<Car> listaCars = LeerListaCars();
            int i;
            for (i = 0; i < listaCars.Count; i++)
            {
                if (listaCars[i].id == id)
                    break;
            }
            return i;
        }

        public void Create(Car car)
        {
            List<Car> listaCars = LeerListaCars();

            // Asignacion de campos de la BD : ID, Fecha_creacion ,etc| SOL Momentania
            if (listaCars.Count >= 1)  // NO vacia
                car.id = listaCars[listaCars.Count - 1].id + 1; // id ultimo + 1
            else
                car.id = 0;

            CultureInfo cultura = CultureInfo.CreateSpecificCulture("en-US");
            car.fecha_creacion = DateTime.Now.ToString("f", cultura);
            listaCars.Add(car);
            ActualizarJSON(listaCars);
        }

        public Car get(int id)
        {
            List<Car> listaCars = LeerListaCars();
            foreach (var c in listaCars)
            {
                if (c.id == id)
                    return c;
            }

            return null;
        }

        public void getAllPrint()
        {
            List<Car> listaCars = LeerListaCars();
            Console.WriteLine("---------------------------------------");
            Console.WriteLine(string.Format("{0,-3} | {1,-11} | {2,-8} | {3,-8}", "Id", "Marca", "Modelo", "Color"));
            Console.WriteLine("---------------------------------------");
            foreach (var c in listaCars)
                Console.WriteLine(c.printear());
            Console.WriteLine("---------------------------------------");
        }

        public void Update(Car car)
        {
            List<Car> listaCars = LeerListaCars();
            int indice = IndicePorID(car.id);

            Car antiguo = listaCars[indice];
            antiguo.Marca = car.Marca;
            antiguo.Modelo = car.Modelo;
            antiguo.Puertas = car.Puertas;
            antiguo.Color = car.Color;
            antiguo.KindCar = car.KindCar;
            ActualizarJSON(listaCars);
        }

        public void Delete(int id)
        {
            List<Car> listaCars = LeerListaCars();
            int indice = IndicePorID(id);
            listaCars.RemoveAt(indice);
            ActualizarJSON(listaCars);
        }

    }
}


