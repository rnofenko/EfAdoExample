using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using AdoExample.DataAccess;
using AdoExample.DataAccess.GenericRepository;
using AdoExample.Models;

namespace AdoExample
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var connection = new SqlConnection("Server=roman;Database=Ado;Trusted_Connection=True;"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = @"
SELECT	c.Id,c.Name, a.Name, g.Name
FROM	Client c
JOIN	Category a
		ON a.Id = c.CatId
JOIN	RiskGroup g
		ON g.Id = c.GroupId
";

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("Id={0},Name={1}", reader[0], reader[1]);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var dataSet = new DataSet();

            using (var connection = new SqlConnection("Server=roman;Database=Ado;Trusted_Connection=True;"))
            {
                connection.Open();

                const string CLIENTS = "SELECT TOP 100 c.Id,c.Name FROM Client c ";
                const string CATEGORIES = "SELECT TOP 100 c.Id,c.Name FROM Category c";
                using (var adapter = new SqlDataAdapter(CLIENTS+CATEGORIES, connection))
                {
                    adapter.Fill(dataSet);
                }
            }

            dataGridView1.DataSource = dataSet;
            dataGridView1.DataMember = dataSet.Tables[0].TableName;

            dataGridView2.DataSource = dataSet;
            dataGridView2.DataMember = dataSet.Tables[1].TableName;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var w = Stopwatch.StartNew();
            LoadClients();
            w.Stop();
            button3.Text = w.Elapsed.ToString();
        }

        public IEnumerable<Client> UseClientRepository()
        {
            var repository = new ClientRepository();
            return repository
                .FindAllBy(x => x.Name.Contains("111"))
                .ToList();
        }

        public IEnumerable<Client> UseUnitOfWork()
        {
            var unit = new UnitOfWork();

            var categories = unit.GetRepository<CategoryRepository>().GetAll();

            return unit.GetRepository<ClientRepository>()
                       .FindAllBy(x => x.Name.Contains("111") && categories.Any(c => c.Id == x.CategoryId))
                       .ToList();
        }

        public void ExplicitLoading()
        {
            using (var context = new EfContext())
            {
                var client = context.Clients.Find(3);

                context.Entry(client)
                    .Reference(x => x.Category)
                    .Load();

                context.Entry(client)
                    .Collection(x => x.Payments)
                    .Load();
            }
        }

        public IEnumerable<Client> EagerLoading()
        {
            using (var context = new EfContext())
            {
                return context
                    .Clients
                    .Where(x => x.Name.Contains("1"))
                    .Where(x => x.GroupId == 3)
                    .OrderByDescending(x => x.Name)
                    .ThenBy(x => x.Changed)
                    .Include(x => x.Category)
                    .Include(x => x.Group)
                    .Skip(5)
                    .Take(10)
                    .ToList();
            }
        }

        public IEnumerable<Client> LoadClients()
        {
            using (var context = new EfContext())
            {
                return context
                    .Clients
                    .Where(x => x.Name.Contains("1"))
                    .Where(x => x.GroupId == 3)
                    .OrderByDescending(x => x.Name)
                    .ThenBy(x => x.Changed)
                    .Skip(5)
                    .Take(10)
                    .ToList();
            }
        }

        private void generateClients()
        {
            for (var i = 0; i < 500; i++)
            {
                using (var context = new EfContext())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Configuration.ValidateOnSaveEnabled = false;

                    for (var j = 0; j < 1000; j++)
                    {
                        var client = new Client
                            {
                                Name = "client" + i + "_" + j,
                                Changed = DateTime.Now,
                                CategoryId = ((j*3 + j/7)%100) + 1,
                                GroupId = ((j*5 + j/11)%100) + 1
                            };
                        context.Entry(client).State = EntityState.Added;
                    }

                    context.SaveChanges();
                }
            }
        }

        private void generateCategories()
        {
            using (var context = new EfContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                context.Configuration.ValidateOnSaveEnabled = false;

                for (int i = 0; i < 100; i++)
                {
                    var category = new Category { Name = "category" + i };
                    context.Entry(category).State = EntityState.Added;
                }

                context.SaveChanges();
            }
        }

        private void generateGroups()
        {
            using (var context = new EfContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                context.Configuration.ValidateOnSaveEnabled = false;

                for (int i = 0; i < 100; i++)
                {
                    var group = new RiskGroup {Name = "group" + i};
                    context.Entry(group).State = EntityState.Added;
                }

                context.SaveChanges();
            }
        }
    }
}