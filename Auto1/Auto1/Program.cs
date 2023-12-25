using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Auto1
{
    class Program
    {
        static int command;
        static List<Auto> cars = new List<Auto>();
        static List<Parking> parks = new List<Parking>();
        static void Main(string[] args)
        {
            Program app = new Program();
            app.Init();
        }

        void Init()
        {
            OpenMenu();
        }

        private void OpenMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("1) Создать машину\n2) Создать парковку\n3) Посмотреть информацию об авто" +
                "\n4) Посмореть свободные места на парковке\n5) Привезти машину на стоянку\n6) Увезти машину из стоянки");
            command = Convert.ToInt32(Console.ReadLine());
            switch (command)
            {
                case 1:
                    CreateCar();
                    break;
                case 2:
                    CreateParking();
                    break;
                case 3:
                    CarsInfo();
                    break;
                case 4:
                    CheckFreePlaces();
                    break;
                case 5:
                    ArriveCarToPark();
                    break;
                case 6:
                    TakeCarFromParkingCommand();
                    break;

            }
        }

        private void Waiting()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nНажмите клавишу, чтобы вернутся в меню.");
            Console.ReadKey();
            OpenMenu();
        }

        private string RandomCarNumber()
        {
            char[] letters = { 'А', 'Б', 'С', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Ц', 'Ч', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
            char[] number = new char[6];
            Random _random = new Random();
            for (int i = 0; i < 6; i++) number[i] = letters[_random.Next(0, letters.Length)];
            string _number = new String(number);
            return _number;
        }

        private void CreateCar()
        {
            Console.Clear();
            Auto car = new Auto(CheckCorrectInfoAnswer("Введите название машины: ", "Введите название машины, а не только цифры: "),
                CheckCorrectInfoAnswer("Введите модель машины: ",
                "Введите название модели, а не только цифры."),
                CheckCorrectInfoAnswer("Введите цвет машины: ", "Введите название цвета, а не цифры."), RandomCarNumber());
            cars.Add(car);
            Console.WriteLine("\n" + car.GetCarInfo());
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\nМашина добавлена в список.");
            Waiting();
        }

        private string CheckCorrectInfoAnswer(string text, string textTrigger)
        {
            Console.WriteLine(text);
            string variable = Convert.ToString(Console.ReadLine());
            while (!SymbolsCount(variable) || variable.All(char.IsDigit))
            {
                if (variable.All(char.IsDigit)) Console.WriteLine(textTrigger);
                variable = Convert.ToString(Console.ReadLine());
                Console.Clear();
            }
            return String.Format(variable);
        }

        private bool SymbolsCount(string variable)
        {
            if (variable.Length > 15)
            {
                Console.WriteLine("Слишком много символов!(max = 15)");
                return false;
            }
            else return true;
        }

        private void CreateParking()
        {
            Console.Clear();
            Console.WriteLine("Введите название паркинга: ");
            string _name = Convert.ToString(Console.ReadLine());
            while (!SymbolsCount(_name))
            {
                _name = Convert.ToString(Console.ReadLine());
                Console.Clear();
            }
            Console.WriteLine("Введите количество мест (от 5 до 20!): ");
            int _places = Convert.ToInt32(Console.ReadLine());
            while (_places < 5 || _places > 20)
            {
                Console.WriteLine("Количество должно быть от 5 до 20 мест!");
                _places = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
            }
            Parking park = new Parking(_name, _places);
            parks.Add(park);
            park.GetParkplacesInfo();
            Waiting();
        }

        private void ObjectsList(bool isCars, string text)
        {
            if (isCars)
            {
                Console.WriteLine(text);
                for (int i = 0; i < cars.Count; i++)
                {
                    Console.WriteLine($"{i + 1}) {cars[i].brand} {cars[i].model}");
                }
                command = Convert.ToInt32(Console.ReadLine()) - 1;
            }
            else
            {
                if (parks.Count == 0)
                {
                    Console.WriteLine("\nНет доступных парковок.");
                    Waiting();
                    return;
                }
                Console.WriteLine(text);
                for (int i = 0; i < parks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}) {parks[i].name}");
                }
                command = Convert.ToInt32(Console.ReadLine()) - 1;
                Console.Clear();
            }
        }

        private void CarsInfo()
        {
            ObjectsList(true, "Выберите машину: ");
            Console.WriteLine(cars[command].GetCarInfo());
            Waiting();
        }

        private void CheckFreePlaces()
        {
            ObjectsList(false, "Выберите парковку: ");
            parks[command].GetParkplacesInfo();
            Waiting();
        }

        private void ArriveCarToPark()
        {
            ObjectsList(true, "Выберите машину: ");
            int car = command;
            ObjectsList(false, "Выберите паркинг: ");
            int park = command;
            parks[park].GetParkplacesInfo();
            Console.WriteLine("Выберите место: ");
            int place = Convert.ToInt32(Console.ReadLine()) - 1;

            cars[car].ParkTheCar(parks[park], place);
            Waiting();
        }

        private void TakeCarFromParkingCommand()
        {
            ObjectsList(true, "Выберите машину: ");
            int car = command;
            if (cars[car].parked == true) cars[car].TakeCarFromParking();
            else Console.WriteLine($"\nМашина {cars[car].brand} {cars[car].model} не припаркована!");
            Waiting();
        }
    }


    static class DateCounter
    {
        public static string DateCount()
        {
            return String.Format(DateTime.Now.ToString());
        }
    }

    class Saves
    {
        string path = Path.GetFullPath(@"D:\savesAuto");
        public void AcivitySaves()
        {
            StreamWriter sw = new StreamWriter(path);
        }


    }
    class Auto
    {
        public string brand;
        public string model;
        public bool parked;
        string color;
        string number;
        string arrivalTime;
        string deptureTime;
        int idPlace;
        Parking parking;

        public Auto(string _brand, string _model, string _color, string _number)
        {
            brand = _brand;
            model = _model;
            color = _color;
            number = _number;
            arrivalTime = null;
            deptureTime = null;
        }

        public string GetCarInfo()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (parked == false) return String.Format($"Бренд: {brand}\nМодель: {model}\nЦвет: {color}\nНомер машины: {number}\nСтатус: Нет на парковке.");
            else return String.Format($"Бренд: {brand}\nМодель: {model}\nЦвет: {color}\nНомер машины: {number}\nВремя прибытия: {arrivalTime}\nСтатус: На парковке." +
                $"\nПарковка: {parking.name}, {idPlace} место.");
        }

        public void ParkTheCar(Parking _parking, int _place)
        {
            if (_parking.busyPlaces[_place] == false)
            {
                parking = _parking;
                parking.busyPlaces[_place] = true;
                parking.placesInfo[_place] = $"{brand} {model}";
                parked = true;
                idPlace = _place;
                arrivalTime = DateCounter.DateCount();
                Console.WriteLine(GetParkInfo(true, parking.name));
            }
            else Console.WriteLine($"\nМесто занято машиной {_parking.placesInfo[_place]}!");
        }

        public void TakeCarFromParking()
        {
            parking.busyPlaces[idPlace] = false;
            parked = false;
            deptureTime = DateCounter.DateCount();
            Console.WriteLine(GetParkInfo(false, parking.name));
        }

        private string GetParkInfo(bool isArrived, string _parkingName)
        {
            if (isArrived) return String.Format($"Автомобиль {brand} {model} был припаркован на стоянку {_parkingName}, {idPlace + 1} место.\nДата: {arrivalTime}");
            if (!isArrived)
            {
                int _idPlace = idPlace;
                idPlace = 0;
                return String.Format($"Автомобиль {brand} {model} был вывезен из парковки {_parkingName}, {_idPlace} место теперь свободно.\nДата: {deptureTime}");
            }
            return null;
        }

    }

    class Parking
    {
        public string name;
        public bool[] busyPlaces;
        public string[] placesInfo;

        public Parking(string _name, int parkingLenght)
        {
            name = _name;
            busyPlaces = new bool[parkingLenght];
            placesInfo = new string[parkingLenght];
        }

        public void GetParkplacesInfo()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Парковка: {name}\nМеста: ");
            string status;
            for (int i = 0; i < busyPlaces.Length; i++)
            {
                if (!busyPlaces[i]) status = "Свободно.";
                else status = $"Занято(Машина: {placesInfo[i]})";
                Console.WriteLine($"{i + 1}) {status}");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
