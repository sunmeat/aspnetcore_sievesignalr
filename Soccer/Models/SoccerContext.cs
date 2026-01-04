using Microsoft.EntityFrameworkCore;

namespace Soccer.Models
{
    public class SoccerContext : DbContext
    {
        public SoccerContext(DbContextOptions<SoccerContext> options)
            : base(options)
        {
            if (Database.EnsureCreated())
            {
                SeedData();
            }
        }

        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<Team> Teams { get; set; } = null!;

        private void SeedData()
        {
            if (Teams.Any())
                return;

            var teams = new[]
            {
                new Team { Name = "Парі Сен-Жермен", Coach = "Луїс Енріке" },
                new Team { Name = "Реал Мадрид", Coach = "Хабі Алонсо" },
                new Team { Name = "Баварія Мюнхен", Coach = "Вінсент Компані" },
                new Team { Name = "Барселона", Coach = "Гансі Флік" },
                new Team { Name = "Арсенал", Coach = "Мікель Артета" },
                new Team { Name = "Манчестер Сіті", Coach = "Пеп Гвардіола" },
                new Team { Name = "Ліверпуль", Coach = "Арне Слот" },
                new Team { Name = "Інтер Мілан", Coach = "Сімоне Індзагі" },
                new Team { Name = "Наполі", Coach = "Антоніо Конте" },
                new Team { Name = "Атлетіко Мадрид", Coach = "Дієго Сімеоне" }
            };

            Teams.AddRange(teams);
            SaveChanges();

            Players.AddRange(
                new Player { Name = "Усман Дембеле", Age = 28, Position = "Правий вінгер", TeamId = Teams.Single(t => t.Name == "Парі Сен-Жермен").Id },
                new Player { Name = "Ламін Ямаль", Age = 18, Position = "Правий вінгер", TeamId = Teams.Single(t => t.Name == "Барселона").Id },
                new Player { Name = "Кіліан Мбаппе", Age = 27, Position = "Форвард", TeamId = Teams.Single(t => t.Name == "Реал Мадрид").Id },
                new Player { Name = "Ерлінг Голанд", Age = 25, Position = "Форвард", TeamId = Teams.Single(t => t.Name == "Манчестер Сіті").Id },
                new Player { Name = "Джамал Мусіяла", Age = 22, Position = "Атакувальний півзахисник", TeamId = Teams.Single(t => t.Name == "Баварія Мюнхен").Id },
                new Player { Name = "Джуд Беллінгем", Age = 22, Position = "Центральний півзахисник", TeamId = Teams.Single(t => t.Name == "Реал Мадрид").Id },
                new Player { Name = "Вінісіус Жуніор", Age = 25, Position = "Лівий вінгер", TeamId = Teams.Single(t => t.Name == "Реал Мадрид").Id },
                new Player { Name = "Вітінья", Age = 25, Position = "Центральний півзахисник", TeamId = Teams.Single(t => t.Name == "Парі Сен-Жермен").Id },
                new Player { Name = "Філ Фоден", Age = 25, Position = "Атакувальний півзахисник", TeamId = Teams.Single(t => t.Name == "Манчестер Сіті").Id },
                new Player { Name = "Гаррі Кейн", Age = 32, Position = "Форвард", TeamId = Teams.Single(t => t.Name == "Баварія Мюнхен").Id },
                new Player { Name = "Кевін Де Брюйне", Age = 34, Position = "Атакувальний півзахисник", TeamId = Teams.Single(t => t.Name == "Манчестер Сіті").Id },
                new Player { Name = "Деклан Райс", Age = 26, Position = "Опорний півзахисник", TeamId = Teams.Single(t => t.Name == "Арсенал").Id },
                new Player { Name = "Віктор Осімхен", Age = 27, Position = "Форвард", TeamId = Teams.Single(t => t.Name == "Наполі").Id },
                new Player { Name = "Хвіча Кварацхелія", Age = 24, Position = "Лівий вінгер", TeamId = Teams.Single(t => t.Name == "Наполі").Id },
                new Player { Name = "Букайо Сака", Age = 24, Position = "Правий вінгер", TeamId = Teams.Single(t => t.Name == "Арсенал").Id },
                new Player { Name = "Флоріан Вірц", Age = 22, Position = "Атакувальний півзахисник", TeamId = Teams.Single(t => t.Name == "Баварія Мюнхен").Id },
                new Player { Name = "Педрі", Age = 23, Position = "Центральний півзахисник", TeamId = Teams.Single(t => t.Name == "Барселона").Id },
                new Player { Name = "Родрі", Age = 29, Position = "Опорний півзахисник", TeamId = Teams.Single(t => t.Name == "Манчестер Сіті").Id },
                new Player { Name = "Мохамед Салах", Age = 33, Position = "Правий вінгер", TeamId = Teams.Single(t => t.Name == "Ліверпуль").Id },
                new Player { Name = "Ендрік", Age = 19, Position = "Форвард", TeamId = Teams.Single(t => t.Name == "Реал Мадрид").Id }
            );

            SaveChanges();
        }
    }
}