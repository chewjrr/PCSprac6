using System.Globalization;
using System.Xml.Linq;


public class Employee
{
    public string FullName { get; set; }
    public int BirthDate { get; set; }
    public List<Job> Jobs { get; set; }
    public List<Salary> Salaries { get; set; }
    public object Positions { get; internal set; }
}

public class Job
{
    public string Position { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Department { get; set; }
}

public class Salary
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Total { get; set; }
}

class Task_1
{

    static void Main()
    {
        string xmlFilePath = "employees.xml";
        if (!File.Exists(xmlFilePath))
        {
            CreateTestXmlFile(xmlFilePath);
        }
        XElement root = XElement.Load(xmlFilePath);
        root.Save(xmlFilePath);

        bool exit = false;

        do
        {
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("Меню команд:");
            Console.WriteLine("1) Поиск сотрудника по фамилии с выводом историю о его трудовой деятельности");
            Console.WriteLine("1.1) Добавление нового сотрудника");
            Console.WriteLine("1.2) Редактирование данных о существующем сотруднике");
            Console.WriteLine("2) Вывод по каждому отделу количества работающих сотрудников и списка должностей (без повторов)");
            Console.WriteLine("3) Вывод сотрудников, которые работают на текущий момент в более чем одном отделе");
            Console.WriteLine("4) Вывод отделов, в которых работает не более 3 сотрудников.");
            Console.WriteLine("5) Вывод годов, в которых было принято и уволено наибольшее и наименьшие количество сотрудников.");
            Console.WriteLine("6) Вывод сотрудников, у которых в этом году юбилей.");
            Console.WriteLine("7) Экспорт данных в XML-файл");
            Console.WriteLine("8) Создание данных о сотруднике(сотрудниках)");
            Console.WriteLine("9) Выход");

            Console.Write("Выберите команду (1-9): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    string lastName;
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write("Введите фамилию сотрудника: ");
                    lastName = Console.ReadLine();

                    DateTime startDate;
                    DateTime endDate;

                    // Ввод даты начала с проверкой на корректность формата
                    do
                    {
                        Console.Write("Введите дату начала (yyyy-mm-dd): ");
                    } while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate));

                    // Ввод даты конца с проверкой на корректность формата и на то, чтобы она была больше или равна дате начала
                    do
                    {
                        Console.Write("Введите дату конца (yyyy-mm-dd): ");
                    } while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate) || endDate < startDate);

                    SearchEmployeeAndDisplayHistory(root, lastName, startDate, endDate);
                    break;
                case "1.1":
                    Console.WriteLine("----------------------------------------------------------------");
                    AddNewEmployeeManual(root);
                    break;

                case "1.2":
                    Console.WriteLine("----------------------------------------------------------------");
                    EditEmployeeManual(root);
                    break;

                case "2":
                    Console.WriteLine("----------------------------------------------------------------");
                    DisplayDepartmentStatistics(root);
                    break;

                case "3":
                    Console.WriteLine("----------------------------------------------------------------");
                    DisplayEmployeesInMultipleDepartments(root);
                    break;

                case "4":
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write("Введите минимальное число сотрудников: ");
                    int minEmployees = int.Parse(Console.ReadLine());
                    DisplayDepartmentsWithFewEmployees(root, minEmployees);
                    break;
                case "5":
                    Console.WriteLine("----------------------------------------------------------------");
                    DisplayYearsWithMostAndFewestEmployees(root);
                    break;

                case "6":
                    Console.WriteLine("----------------------------------------------------------------");
                    for (int i = 20; i <= 100; i += 5)
                    {
                        DisplayEmployeesWithAnniversary(root, i);
                    }
                    break;

                case "7":
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write("Введите имя файла для экспорта (out.xml): ");
                    string exportFileName = Console.ReadLine();
                    ExportDepartmentDataToXml(root, exportFileName);
                    break;

                case "8":
                    int count;
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write("Введите количество сотрудников для генерации: ");
                    count = int.Parse(Console.ReadLine());
                    generatorEmployees(root, count);
                    Console.WriteLine($"Сгенерировано {count}");
                    break;

                case "9":
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }

        } while (!exit);
    }
    static void Main1()
    {
        string xmlFilePath = "employees.xml";
        if (!File.Exists(xmlFilePath))
        {
            CreateTestXmlFile(xmlFilePath);
        }
        XElement root = XElement.Load(xmlFilePath);
        root.Save(xmlFilePath);

        int count = 1000;
        // generatorEmployees(root, count);

        Console.WriteLine("----------------------------------------------------------------");
        SearchEmployeeAndDisplayHistory(root, "Калита", new DateTime(2017, 01, 01), new DateTime(2023, 12, 31));
        Console.WriteLine("----------------------------------------------------------------");
        DisplayDepartmentStatistics(root);
        Console.WriteLine("----------------------------------------------------------------");
        DisplayEmployeesInMultipleDepartments(root);
        Console.WriteLine("----------------------------------------------------------------");
        DisplayDepartmentsWithFewEmployees(root, 3);
        Console.WriteLine("----------------------------------------------------------------");
        DisplayYearsWithMostAndFewestEmployees(root);
        Console.WriteLine("----------------------------------------------------------------");
        DisplayEmployeesWithAnniversary(root, 20);
        DisplayEmployeesWithAnniversary(root, 30);
        Console.WriteLine("----------------------------------------------------------------");
        ExportDepartmentDataToXml(root, "exported_data.xml");
        Console.WriteLine("----------------------------------------------------------------");
    }

    static void generatorEmployees(XElement root, int count)
    {
        for (int i = 0; i < count; i++)
        {
            XElement randomEmployee = DataGenerator.GenerateRandomEmployee();
            root.Add(randomEmployee);
        }
        root.Save("employees.xml");
    }

    static void CreateTestXmlFile(string filePath)
    {
        XElement root = new XElement("Employees",
            new XElement("Сотрудник",
                new XElement("ФИО", "Калита Иван Данилович"),
                new XElement("Год_рождения", "2003"),
                new XElement("Список_Работ",
                    new XElement("Работа",
                        new XElement("Название должности", "Работник отдела Инжиниринг"),
                        new XElement("Дата начала", "01.03.2021"),
                        new XElement("Дата окончания", "06.03.2021"),
                        new XElement("Отдел", "Отдел Инжиниринг")
                    ),
                    new XElement("Работа",
                        new XElement("Название должности", "Начальник инжиниров"),
                        new XElement("Дата начала", "06.03.2021"),
                        new XElement("Дата окончания", "27.08.2022"),
                        new XElement("Отдел", "Отдел Инжиниринг")
                    ),
                    new XElement("Работа",
                        new XElement("Название должности", "Директор Начальников Инжиниров"),
                        new XElement("Дата начала", "28.08.2022"),
                        new XElement("Отдел", "Администрация")
                    )
                ),
                new XElement("Список Зарплат",
                    new XElement("Зарплата",
                        new XElement("Год", "2021"),
                        new XElement("Месяц", "03"),
                        new XElement("Итого", "5678")
                    ),
                    new XElement("Зарплата",
                        new XElement("Год", "2022"),
                        new XElement("Месяц", "05"),
                        new XElement("Итого", "16000")
                    ),
                    new XElement("Зарплата",
                        new XElement("Год", "2023"),
                        new XElement("Месяц", "09"),
                        new XElement("Итого", "500009")
                    )
                )
            )
        );
        root.Save(filePath);
    }

    static void SearchEmployeeAndDisplayHistory(XElement root, string lastName, DateTime startDate, DateTime endDate)
    {
        var employee = root.Elements("Сотрудник")
            .Where(e => e.Element("ФИО").Value.Contains(lastName))
            .FirstOrDefault();

        if (employee != null)
        {
            Console.WriteLine($"=> Сотрудник {employee.Element("ФИО").Value}:");
            // Дата рождения
            Console.WriteLine($"Дата рождения: {employee.Element("Год рождения").Value}");

            // История трудовой деятельности
            var workHistory = employee.Descendants("Работа")
                .Where(r => IsDateInRange(r.Element("Дата начала").Value, startDate, endDate))
                .OrderBy(r => DateTime.Parse(r.Element("Дата начала").Value));

            Console.WriteLine("История трудовой деятельности:");
            foreach (var work in workHistory)
            {
                Console.WriteLine($"Должность: {work.Element("Название должности").Value}");
                Console.WriteLine($"Дата начала работы: {work.Element("Дата начала").Value}");
                Console.WriteLine($"Дата окончания работы: {work.Element("Дата_окончания")?.Value ?? "настоящее время"}");
                Console.WriteLine($"Отдел: {work.Element("Отдел").Value}");
                Console.WriteLine();
            }

            // Начисления заработной платы
            var salaryHistory = employee.Descendants("Зарплата")
            .Where(s => IsDateInRange($"{s.Element("Год").Value}-{s.Element("Месяц").Value}", startDate, endDate))
            .OrderBy(s => DateTime.Parse($"{s.Element("Год").Value}-{s.Element("Месяц").Value}"));

            if (salaryHistory.Any())
            {
                Console.WriteLine("История начислений заработной платы:");
                foreach (var salary in salaryHistory)
                {
                    Console.WriteLine($"Год: {salary.Element("Год").Value}, Месяц: {salary.Element("Месяц").Value}, Итого: {salary.Element("Итого").Value}");
                }

                // Расчет максимального, минимального и среднего значения заработной платы
                var salaries = salaryHistory.Select(s => int.Parse(s.Element("Итого").Value));
                Console.WriteLine($"Максимальная зарплата: {salaries.Max()}");
                Console.WriteLine($"Минимальная зарплата: {salaries.Min()}");
                Console.WriteLine($"Средняя зарплата: {salaries.Average()}");
            }
            else
            {
                Console.WriteLine("История начислений заработной платы отсутствует.");
            }
        }
        else
        {
            Console.WriteLine($"Сотрудник с фамилией {lastName} не найден.");
        }
    }


    static void AddNewEmployeeManual(XElement root)
    {
        Console.WriteLine("Добавление нового сотрудника:");

        // Запрашиваем у пользователя данные
        Console.Write("Введите ФИО сотрудника: ");
        string name = Console.ReadLine();

        Console.Write("Введите год рождения сотрудника: ");
        int birthYear = int.Parse(Console.ReadLine());

        // Добавляем проверку, существует ли такой сотрудник с ФИО и годом рождения
        var employee1 = root.Elements("Сотрудник")
            .Where(e => e.Element("ФИО").Value == name && int.Parse(e.Element("Год_рождения").Value) == birthYear)
            .FirstOrDefault();
        // Если существует, то пишем, что надо заново ввести данные
        if (employee1 != null)
        {
            Console.WriteLine("Такой сотрудник уже существует. Попробуйте заново.");
            return;
        }

        // Создаем новый элемент сотрудника
        XElement employee = new XElement("Сотрудник",
            new XElement("ФИО", name),
            new XElement("Год рождения", birthYear),
            new XElement("Список Работ"),
            new XElement("Список Зарплат")
        );

        // Добавляем работу в существующий элемент <Работа> в <Список_Работ>
        Console.WriteLine("Добавление трудовой деятельности:");
        XElement workHistory = new XElement("Работа");

        do
        {
            Console.Write("Введите название должности: ");
            string position = Console.ReadLine();

            Console.Write("Введите дату начала работы: ");
            string startDate = Console.ReadLine();

            Console.Write("Введите дату окончания работы (если работает по настоящее время, оставьте поле пустым): ");
            string endDate = Console.ReadLine();

            Console.Write("Введите название отдела: ");
            string department = Console.ReadLine();

            workHistory.Add(
                new XElement("Название_должности", position),
                new XElement("Дата_начала", startDate),
                new XElement("Дата_окончания", endDate),
                new XElement("Отдел", department)
            );
        } while (!string.IsNullOrEmpty(workHistory.Element("Дата_окончания")?.Value) &&
                 DateTime.Parse(workHistory.Element("Дата_начала").Value) >
                 DateTime.Parse(workHistory.Element("Дата_окончания").Value));

        // Добавляем workHistory в employee
        employee.Element("Список_Работ").Add(workHistory);

        // Добавляем сотрудника в корневой элемент
        root.Add(employee);
        root.Save("employees.xml");
    }



    static void EditEmployeeManual(XElement root)
    {
        Console.WriteLine("Редактирование сотрудника:");
        Console.Write("Введите ФИО сотрудника: ");
        string name = Console.ReadLine();

        var employee = root.Elements("Сотрудник")
            .Where(e => e.Element("ФИО").Value == name)
            .FirstOrDefault();

        if (employee == null)
        {
            Console.WriteLine($"Сотрудник с ФИО {name} не найден.");
            return;
        }

        SearchEmployeeAndDisplayHistory(root, name, new DateTime(1000, 01, 01), new DateTime(9999, 12, 31));

        Console.WriteLine("Выберите пункт для редактирования:");
        Console.WriteLine("1) ФИО");
        Console.WriteLine("2) Год рождения");
        Console.WriteLine("3) Список работ");
        Console.WriteLine("4) Список зарплат");
        Console.Write("Введите номер пункта: ");
        int choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case 1:
                Console.Write("Введите новое ФИО: ");
                string newFIO = Console.ReadLine();
                employee.Element("ФИО").Value = newFIO;
                Console.WriteLine("Информация о сотруднике после редактирования:");
                SearchEmployeeAndDisplayHistory(root, newFIO, new DateTime(1000, 01, 01), new DateTime(9999, 12, 31));
                break;

            case 2:
                Console.Write("Введите новый год рождения: ");
                int newBirthYear = int.Parse(Console.ReadLine());
                employee.Element("Год_рождения").Value = newBirthYear.ToString();
                Console.WriteLine("Информация о сотруднике после редактирования:");
                SearchEmployeeAndDisplayHistory(root, employee.Element("ФИО").Value, new DateTime(1000, 01, 01), new DateTime(9999, 12, 31));
                break;

            case 3:
                // Редактирование Списка работ
                Console.WriteLine("Список работ:");
                var workList = employee.Element("Список_Работ").Elements("Работа").ToList();

                for (int i = 0; i < workList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {workList[i].Element("Название_должности").Value}");
                }

                Console.Write("Выберите номер работы для редактирования: ");
                int workIndex = int.Parse(Console.ReadLine()) - 1;

                if (workIndex >= 0 && workIndex < workList.Count)
                {
                    Console.WriteLine("Выберите действие:");
                    Console.WriteLine("1. Изменить данные работы");
                    Console.WriteLine("2. Удалить работу");
                    Console.WriteLine("3. Добавить новую работу");

                    int workAction = int.Parse(Console.ReadLine());

                    if (workAction == 1)
                    {
                        // Изменение данных работы
                        Console.Write("Введите новое название должности: ");
                        string newPosition = Console.ReadLine();

                        Console.Write("Введите новую дату начала работы: ");
                        string newStartDate = Console.ReadLine();

                        Console.Write("Введите новую дату окончания работы (если работает по настоящее время, оставьте поле пустым): ");
                        string newEndDate = Console.ReadLine();

                        Console.Write("Введите новое название отдела: ");
                        string newDepartment = Console.ReadLine();

                        workList[workIndex].Element("Название_должности").Value = newPosition;
                        workList[workIndex].Element("Дата_начала").Value = newStartDate;
                        workList[workIndex].Element("Дата_окончания").Value = newEndDate;
                        workList[workIndex].Element("Отдел").Value = newDepartment;

                        // Вывод данных о сотруднике
                        Console.WriteLine("Информация о сотруднике после редактирования:");
                        SearchEmployeeAndDisplayHistory(root, employee.Element("ФИО").Value, new DateTime(1000, 01, 01), new DateTime(9999, 12, 31));
                    }
                    else if (workAction == 2)
                    {
                        // Удаление работы
                        workList[workIndex].Remove();

                        // Вывод данных о сотруднике
                        Console.WriteLine("Информация о сотруднике после редактирования:");
                        SearchEmployeeAndDisplayHistory(root, employee.Element("ФИО").Value, new DateTime(1000, 01, 01), new DateTime(9999, 12, 31));
                    }
                    else if (workAction == 3)
                    {
                        // Добавление новой работы
                        Console.Write("Введите название должности: ");
                        string newPosition = Console.ReadLine();

                        Console.Write("Введите дату начала работы: ");
                        string newStartDate = Console.ReadLine();

                        Console.Write("Введите дату окончания работы (если работает по настоящее время, оставьте поле пустым): ");
                        string newEndDate = Console.ReadLine();

                        Console.Write("Введите название отдела: ");
                        string newDepartment = Console.ReadLine();

                        XElement newWork = new XElement("Работа",
                            new XElement("Название_должности", newPosition),
                            new XElement("Дата_начала", newStartDate),
                            new XElement("Дата_окончания", newEndDate),
                            new XElement("Отдел", newDepartment)
                        );

                        employee.Element("Список_Работ").Add(newWork);

                        // Вывод данных о сотруднике
                        Console.WriteLine("Информация о сотруднике после редактирования:");
                        SearchEmployeeAndDisplayHistory(root, employee.Element("ФИО").Value, new DateTime(1000, 01, 01), new DateTime(9999, 12, 31));
                    }
                }
                else
                {
                    Console.WriteLine("Некорректный номер работы.");
                }
                break;

            case 4:
                Console.WriteLine("Список зарплат:");
                var salaryList = employee.Element("Список_Зарплат").Elements("Зарплата").ToList();

                for (int i = 0; i < salaryList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Год: {salaryList[i].Element("Год").Value}, Месяц: {salaryList[i].Element("Месяц").Value}, Итого: {salaryList[i].Element("Итого").Value}");
                }

                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1) Изменить данные зарплаты");
                Console.WriteLine("2) Удалить зарплату");
                Console.WriteLine("3) Добавить новую зарплату");

                int salaryAction = int.Parse(Console.ReadLine());

                if (salaryAction == 1)
                {
                    Console.Write("Введите номер зарплаты для редактирования: ");
                    int salaryIndex = int.Parse(Console.ReadLine()) - 1;

                    if (salaryIndex >= 0 && salaryIndex < salaryList.Count)
                    {
                        Console.Write("Введите новый год: ");
                        string newYear = Console.ReadLine();

                        Console.Write("Введите новый месяц: ");
                        string newMonth = Console.ReadLine();

                        Console.Write("Введите новую сумму зарплаты: ");
                        string newAmount = Console.ReadLine();

                        salaryList[salaryIndex].Element("Год").Value = newYear;
                        salaryList[salaryIndex].Element("Месяц").Value = newMonth;
                        salaryList[salaryIndex].Element("Итого").Value = newAmount;

                        Console.WriteLine("Информация о сотруднике после редактирования:");
                        SearchEmployeeAndDisplayHistory(root, employee.Element("ФИО").Value, new DateTime(1000, 01, 01), new DateTime(9999, 12, 31));
                    }
                    else
                    {
                        Console.WriteLine("Некорректный номер зарплаты.");
                    }
                }
                else if (salaryAction == 2)
                {
                    Console.Write("Введите номер зарплаты для удаления: ");
                    int salaryIndex = int.Parse(Console.ReadLine()) - 1;

                    if (salaryIndex >= 0 && salaryIndex < salaryList.Count)
                    {
                        salaryList[salaryIndex].Remove();

                        Console.WriteLine("Информация о сотруднике после редактирования:");
                        SearchEmployeeAndDisplayHistory(root, employee.Element("ФИО").Value, new DateTime(1000, 01, 01), new DateTime(9999, 12, 31));
                    }
                    else
                    {
                        Console.WriteLine("Некорректный номер зарплаты.");
                    }
                }
                else if (salaryAction == 3)
                {
                    Console.Write("Введите год: ");
                    string newYear = Console.ReadLine();

                    Console.Write("Введите месяц: ");
                    string newMonth = Console.ReadLine();

                    Console.Write("Введите сумму зарплаты: ");
                    string newAmount = Console.ReadLine();

                    XElement newSalary = new XElement("Зарплата",
                        new XElement("Год", newYear),
                        new XElement("Месяц", newMonth),
                        new XElement("Итого", newAmount)
                    );

                    employee.Element("Список_Зарплат").Add(newSalary);

                    Console.WriteLine("Информация о сотруднике после редактирования:");
                    SearchEmployeeAndDisplayHistory(root, employee.Element("ФИО").Value, new DateTime(1000, 01, 01), new DateTime(9999, 12, 31));
                }
                else
                {
                    Console.WriteLine("Некорректный выбор.");
                }
                break;

            default:
                Console.WriteLine("Некорректный выбор.");
                break;
        }

        root.Save("employees.xml");
    }


    static bool IsDateInRange(string dateString, DateTime startDate, DateTime endDate)
    {
        DateTime date = DateTime.Parse(dateString);
        return date >= startDate && date <= endDate;
    }

    static void DisplayDepartmentStatistics(XElement root)
    {
        var departments = root.Descendants("Работа")
            .GroupBy(r => r.Element("Отдел").Value);

        Console.WriteLine("Статистика по отделам:");

        foreach (var department in departments)
        {
            string departmentName = department.Key;

            var employeesInDepartment = department
                .Count(r => string.IsNullOrEmpty(r.Element("Дата_окончания")?.Value));

            var totalEmployeesInDepartment = department
                .Select(r => r.Parent.Parent.Element("ФИО").Value)
                .Distinct()
                .Count();

            var totalEmployeesInCompany = root.Elements("Сотрудник")
                .Count();

            double percentageOfWorkingEmployees = totalEmployeesInCompany == 0 ? 0 : (double)employeesInDepartment / totalEmployeesInCompany * 100;

            Console.WriteLine($"Отдел: {departmentName}");
            Console.WriteLine($"Количество сотрудников: {employeesInDepartment}");
            Console.WriteLine($"Общее количество сотрудников: {totalEmployeesInDepartment}");
            Console.WriteLine($"Список должностей: {string.Join(", ", department.Select(r => r.Element("Название_должности").Value))}");
            Console.WriteLine($"Доля сотрудников в отделе от общего числа сотрудников в компании: {percentageOfWorkingEmployees:F2}%");
            Console.WriteLine();
        }
    }



    static void DisplayEmployeesInMultipleDepartments(XElement root)
    {
        var employees = root.Elements("Сотрудник")
            .Where(e => e.Descendants("Работа").Any(r => string.IsNullOrEmpty(r.Element("Дата_окончания")?.Value)))
            .Where(e => e.Descendants("Работа").GroupBy(r => r.Element("Отдел").Value).Any(g => g.Count() > 1));

        Console.WriteLine("Сотрудники, работающие в более чем одном отделе:");
        foreach (var employee in employees)
        {
            Console.WriteLine($"ФИО: {employee.Element("ФИО").Value}");

            var departments = employee.Descendants("Работа")
                .Where(r => string.IsNullOrEmpty(r.Element("Дата_окончания")?.Value))
                .Select(r => r.Element("Отдел").Value)
                .Distinct();

            Console.WriteLine($"Отделы: {string.Join(", ", departments)}");
            Console.WriteLine();
        }
    }
    static void DisplayDepartmentsWithFewEmployees(XElement root, int maxEmployees)
    {
        var departments = root.Descendants("Работа")
            .GroupBy(r => r.Element("Отдел").Value)
            .Where(g => g.Count() <= maxEmployees)
            .Select(g => g.Key);

        Console.WriteLine($"Отделы, в которых работает не более {maxEmployees} сотрудников:");
        foreach (var department in departments)
        {
            Console.WriteLine(department);
        }
    }
    static void DisplayYearsWithMostAndFewestEmployees(XElement root)
    {
        var hireYears = root.Descendants("Работа")
            .Where(r => !string.IsNullOrEmpty(r.Element("Дата_начала").Value))
            .GroupBy(r => DateTime.Parse(r.Element("Дата_начала").Value).Year)
            .Select(g => new { Year = g.Key, Count = g.Count() });

        var fireYears = root.Descendants("Работа")
            .Where(r => !string.IsNullOrEmpty(r.Element("Дата_окончания")?.Value))
            .GroupBy(r => DateTime.Parse(r.Element("Дата_окончания").Value).Year)
            .Select(g => new { Year = g.Key, Count = g.Count() });

        var mostHiresYear = hireYears.OrderByDescending(g => g.Count).FirstOrDefault();
        var fewestHiresYear = hireYears.OrderBy(g => g.Count).FirstOrDefault();

        var mostFiresYear = fireYears.OrderByDescending(g => g.Count).FirstOrDefault();
        var fewestFiresYear = fireYears.OrderBy(g => g.Count).FirstOrDefault();

        Console.WriteLine("Годы с наибольшим и наименьшим количеством принятых сотрудников:");
        Console.WriteLine($"Наибольшее количество принятых сотрудников было в {mostHiresYear?.Year} году: {mostHiresYear?.Count ?? 0} сотрудник(ов)");
        Console.WriteLine($"Наименьшее количество принятых сотрудников было в {fewestHiresYear?.Year} году: {fewestHiresYear?.Count ?? 0} сотрудник(ов)");

        Console.WriteLine("\nГоды с наибольшим и наименьшим количеством уволенных сотрудников:");
        Console.WriteLine($"Наибольшее количество уволенных сотрудников было в {mostFiresYear?.Year} году: {mostFiresYear?.Count ?? 0} сотрудник(ов)");
        Console.WriteLine($"Наименьшее количество уволенных сотрудников было в {fewestFiresYear?.Year} году: {fewestFiresYear?.Count ?? 0} сотрудник(ов)");
    }
    static void DisplayEmployeesWithAnniversary(XElement root, int anniversaryAge)
    {
        int currentYear = DateTime.Now.Year;

        var employeesWithAnniversary = root.Elements("Сотрудник")
            .Where(e => e.Element("Год_рождения") != null)
            .Where(e =>
            {
                var birthYearElement = e.Element("Год_рождения");
                if (birthYearElement != null)
                {
                    if (int.TryParse(birthYearElement.Value, out int birthYear))
                    {
                        return currentYear - birthYear == anniversaryAge;
                    }
                }
                return false;
            });

        Console.WriteLine($"Сотрудники, у которых в этом году {anniversaryAge} летный юбилей:");

        foreach (var employee in employeesWithAnniversary)
        {
            string fullName = employee.Element("ФИО").Value;
            int birthYear = int.Parse(employee.Element("Год_рождения").Value);
            int age = currentYear - birthYear;

            Console.WriteLine($"ФИО: {fullName}, Возраст: {age} лет");
        }
    }


    static void ExportDepartmentDataToXml(XElement root, string outputFilePath)
    {
        var departments = root.Descendants("Работа")
            .GroupBy(r => r.Element("Отдел").Value);

        XElement departmentsXml = new XElement("Отделы");

        foreach (var department in departments)
        {
            string departmentName = department.Key;
            var workingEmployeesCount = department
                .Where(r => string.IsNullOrEmpty(r.Element("Дата_окончания")?.Value))
                .Select(r => r.Parent.Parent)
                .Distinct()
                .Count();

            var youngEmployeesCount = department
                .Where(r => string.IsNullOrEmpty(r.Element("Дата_окончания")?.Value))
                .Select(r => r.Parent.Parent.Element("Год_рождения").Value)
                .Distinct()
                .Count(g => CalculateAge(g) <= 30);

            XElement departmentXml = new XElement("Отдел",
                new XAttribute("Название", departmentName),
                new XElement("Количество сотрудников", workingEmployeesCount),
                new XElement("Количество сотрудников(молодые)", youngEmployeesCount)
            );

            departmentsXml.Add(departmentXml);
        }

        // Сохранение данных в XML-файл
        departmentsXml.Save(outputFilePath);

        Console.WriteLine($"Данные успешно экспортированы в файл: {outputFilePath}");
    }
    static int CalculateAge(string birthDate)
    {
        int currentYear = DateTime.Now.Year;
        int birthYear = int.Parse(birthDate);
        return currentYear - birthYear;
    }

};



class DataGenerator
{
    private static Random random = new Random();

    public static XElement GenerateRandomEmployee()
    {
        XElement employee = new XElement("Сотрудник",
            new XElement("ФИО", GenerateRandomFullName()),
            new XElement("Год_рождения", GenerateRandomBirthYear()),
            new XElement(GenerateRandomWorkHistory()),
            new XElement(GenerateRandomSalaryHistory())
        );

        return employee;
    }

    // данные в файлах firфамилии - surname.txt имена - firstname.txt отчества - middlename.txt
    private static string GenerateRandomFullName()
    {
        string surname_file = "surname.txt";
        string firstname_file = "firstname.txt";
        string middlename_file = "middlename.txt";
        // Получить длину файла и генерировать случайные данные в пределах длины файла
        int surname_length = File.ReadAllLines(surname_file).Length;
        int firstname_length = File.ReadAllLines(firstname_file).Length;
        int middlename_length = File.ReadAllLines(middlename_file).Length;

        string surname = File.ReadAllLines(surname_file)[random.Next(0, surname_length)];
        string firstname = File.ReadAllLines(firstname_file)[random.Next(0, firstname_length)];
        string middlename = File.ReadAllLines(middlename_file)[random.Next(0, middlename_length)];

        // Если первая буква строчная, то сделать заглавной в трёх переменных
        if (char.IsLower(surname[0]))
        {
            surname = char.ToUpper(surname[0]) + surname.Substring(1);
        }
        if (char.IsLower(firstname[0]))
        {
            firstname = char.ToUpper(firstname[0]) + firstname.Substring(1);
        }
        if (char.IsLower(middlename[0]))
        {
            middlename = char.ToUpper(middlename[0]) + middlename.Substring(1);
        }

        return $"{surname} {firstname} {middlename}";
    }

    private static string GenerateRandomBirthYear()
    {
        int currentYear = DateTime.Now.Year;
        int birthYear = random.Next(1950, 2004);
        return $"{birthYear}";
    }

    private static XElement GenerateRandomWorkHistory()
    {
        XElement workHistory = new XElement("Список_Работ");

        int workCount = random.Next(1, 15);

        for (int i = 0; i < workCount; i++)
        {
            DateTime startDate = GenerateRandomStartDate();
            DateTime endDate = GenerateRandomEndDate(startDate.Year);
            string endDateString = (endDate == new DateTime(1000, 1, 1)) ? string.Empty : endDate.ToString("dd.MM.yyyy");

            XElement work = new XElement("Работа",
                               new XElement("Название_должности", GenerateRandomPosition()),
                                              new XElement("Дата_начала", startDate.ToString("dd.MM.yyyy")),
                                                             new XElement("Дата_окончания", endDateString),
                                                                            new XElement("Отдел", GenerateRandomDepartment())
                                                                       );

            workHistory.Add(work);
        }

        return workHistory;
    }

    // Должности должны соответствовать отделам
    private static string GenerateRandomPosition()
    {
        string position_file = "position.txt";
        int position_length = File.ReadAllLines(position_file).Length;
        string position = File.ReadAllLines(position_file)[random.Next(0, position_length)];
        return $"{position}";
    }

    private static DateTime GenerateRandomStartDate()
    {
        int currentYear = DateTime.Now.Year;
        int startYear = random.Next(1950, currentYear);
        int startMonth = random.Next(1, 13);
        int startDay = random.Next(1, DateTime.DaysInMonth(startYear, startMonth) + 1);

        return new DateTime(startYear, startMonth, startDay);
    }

    private static DateTime GenerateRandomEndDate(int startYear)
    {
        // Генерируем случайное число (0 или 1)
        int randomNumber = random.Next(2);

        if (randomNumber == 0)
        {
            // Возвращаем пустую строку
            return new DateTime(1000, 1, 1);
        }
        else
        {
            // Генерируем дату окончания
            int endYear = random.Next(startYear, DateTime.Now.Year + 1);
            int endMonth = random.Next(1, 13);
            int endDay = random.Next(1, DateTime.DaysInMonth(endYear, endMonth) + 1);

            return new DateTime(endYear, endMonth, endDay);
        }
    }


    private static string GenerateRandomDepartment()
    {
        string department_file = "department.txt";
        int department_length = File.ReadAllLines(department_file).Length;
        string department = File.ReadAllLines(department_file)[random.Next(0, department_length)];
        return $"{department}";
    }

    private static XElement GenerateRandomSalaryHistory()
    {
        XElement salaryHistory = new XElement("Список_Зарплат");

        int salaryCount = random.Next(1, 5);

        for (int i = 0; i < salaryCount; i++)
        {
            XElement salary = new XElement("Зарплата",
                                              new XElement("Год", GenerateRandomSalaryYear()),
                                                                                           new XElement("Месяц", GenerateRandomSalaryMonth()),
                                                                                                                                                       new XElement("Итого", GenerateRandomSalaryTotal())
                                                                                                                                                                                                                                             );

            salaryHistory.Add(salary);
        }

        return salaryHistory;
    }

    private static string GenerateRandomSalaryYear()
    {
        int currentYear = DateTime.Now.Year;
        int salaryYear = random.Next(1950, currentYear);
        return $"{salaryYear}";
    }

    private static string GenerateRandomSalaryMonth()
    {
        int salaryMonth = random.Next(1, 13);
        return $"{salaryMonth}";
    }

    private static string GenerateRandomSalaryTotal()
    {
        int salaryTotal = random.Next(10000, 1000000);
        return $"{salaryTotal}";
    }
}