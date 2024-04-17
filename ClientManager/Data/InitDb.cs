namespace ClientManager.Data
{

    public class InitDb()
    {
        public static void InitData(IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<ClientDbContext>());
            }
        }
        private static void SeedData(ClientDbContext context)
        {
            if(!context.Clients.Any())
            {
                Console.WriteLine(" --- seeding data ---");
                context.Clients.AddRange(
                    new Models.Client(){Id=Guid.NewGuid() ,Name="Jakub", Surname="Kowalski", },
                    new Models.Client(){Id=Guid.NewGuid() ,Name="Alan", Surname="Bratek", },
                    new Models.Client(){Id=Guid.NewGuid() ,Name="Janina", Surname="Garbacka", }
                    );
                    context.SaveChanges();
            }
            else{
                Console.WriteLine("--- already got data  ---");
            }
        }
    }   

}
