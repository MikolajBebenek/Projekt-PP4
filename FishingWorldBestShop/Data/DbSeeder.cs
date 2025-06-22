using System.Text;
using FishingWorldEShop.Models;
using FishingWorldEShop.Repositories;

namespace FishingWorldEShop
{
    public static class DbSeeder
    {
        public static void Seed(FishingWorldEShopContext context)
        {
            if (!context.Customers.Any())
            {
                context.Customers.AddRange(
                    new Customer
                    {
                        Name = "Jan",
                        Surname = "Wędkarz",
                        Email = "jan@wedkarz.pl",
                        Phone = "123456789",
                        Address = "ul. Rybacka 12, 00-000 Warszawa"
                    },
                    new Customer
                    {
                        Name = "Anna",
                        Surname = "Łowczyni",
                        Email = "anna@lowy.pl",
                        Phone = "987654321",
                        Address = "ul. Łososiowa 5, 00-001 Kraków"
                    }
                );
                context.SaveChanges();
            }

            if (!context.Categories.Any())
            {
                var spinning = new Category { Name = "Spinning" };
                var gruntowa = new Category { Name = "Gruntowa" };
                var splawikowa = new Category { Name = "Splawikowa" };

                context.Categories.AddRange(spinning, gruntowa, splawikowa);
                context.SaveChanges();

                if (!context.Products.Any())
                {
                    context.Products.AddRange(
                        new Product
                        {
                            Name = "Wędka Savage Gear SG2 Ultra Light Game",
                            Description = "W kolekcji tej prezentujemy doskonałe wedki spinningowe Savage Gear SG2 Ultra Light Game, SG2 Light Game i SG2 Medium Game. Wśród wędek z tej grupy znajdziemy lekkie okoniowo – pstrągowe klasyki pozwalające łowić bardzo lekkimi przynętami na każdym praktycznie rodzaju łowiska. Od niewielkiego stawu, poprzez jeziora aż do rzek średniej wielkości. Oprócz lekkich kijów, w tej grupie proponujemy wędki o średniej masie wyrzutu. Wędki te doskonale sprawdzą się podczas wypraw na sandacze i szczupaki. Jak wszystkie wędki Savage Gear serii SG2 tak i modele z tej grupy wyposażono w doskonałe przelotki Seaguide z pierścieniami ze stali nierdzewnej typu Coil Control System. Na uwagę zasługuje także profesjonalny i ergonomiczny uchwyt kołowrotka typu ALIEN, wykonany z włókna węglowego, dzięki któremu dłoń wędkarza ma bezpośredni kontakt z blankiem wędki. Prezentowane w tej grupie wędki z serii SG2 posiadają doskonałe blanki wykonane z włókien węglowych japońskiej marki Toray. Wędek Savage Gear z tej serii można używać do łowienia okoni, sandaczy i szczupaków. Doskonale sprawdzają się podczas łowienia średniej wielkości gumami, wahadłówkami oraz woblerami do Twitchingu. Materiały wykorzystane do wykończenia elementów dolnika charakteryzują się wysoką odpornością na uszkodzenia mechaniczne. Wysoka odporność na trudy użytkowania to zresztą cecha wszystkich wędek z serii SG2. Wysoka jakość, trwałość i relatywnie niska cena. Takie są właśnie wędki Savage Gear SG2.",
                            Price = 244.00m,
                            CategoryId = spinning.Id,
                            EAN = 143799
                        },
                        new Product
                        {
                            Name = "Wędka Robinson Stinger Feeder",
                            Description = "Seria wędek Stinger została wykonana z włókna węglowego IM 6. Bardzo charakterystyczne efektowne perłowo-niebieskie malowanie zwraca uwagę. Uzbrojone w przelotki typu TS, zaopatrzone w korkowo-neoprenową rękojeść. Docenione przez wędkarzy ze względu na sprężyste blanki i komfort użytkowania.",
                            Price = 199.00m,
                            CategoryId = gruntowa.Id,
                            EAN = 162057
                        },
                        new Product
                        {
                            Name = "Wędka Robinson Kinetik RS Tele Float",
                            Description = "Wędki Robinson Kinetik rozpoznamy po zestawieniu czerni z czerwienią. Są to nowoczesne, świetne jakościowo kije, przy tym dostępne za dość niską cenę. Nowa generacja została znacznie podrasowana, ulepszono parametry wytrzymałościowe. Wzmocniono strukturę blanku, szczególnie zadbano o dolną część - 25% więcej mocy. Mimo to wędki nie straciły na lekkości i sprężystości. Szybka akcja i zwiększona moc sprzyjają zacięciu z większej odległości. Teraz bez problemu wyholujemy też znacznie większe ryby.",
                            Price = 179.00m,
                            CategoryId = splawikowa.Id,
                            EAN = 169228
                        }
                    );

                    context.SaveChanges();
                }
            }
        }
    }
}
