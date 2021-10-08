using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmiralBatti
{
    class Program
    {
        //Rastgele gemi konumları için random sınıfı
        static Random random = new Random();
        class Nokta
        {
            public int x { get; set; }
            public int y { get; set; }
            //Noktaların x ve y değerleri
            public bool vuruldumu { get; set; }
            //O noktanın vurulup vurulmadığını belirten değişken
            public Nokta() { x = 0;y = 0; }
            public Nokta(int x,int y)
            {
                //Yapıcı metot
                this.x = x;
                this.y = y;
                vuruldumu = false;
            }
        }
        class Gemi
        {
            //Gemi sınıfı
            //Gövdelerini belirten gövde dizisi
            public Nokta[] govde{ get; set; }
            public int boyut { get; set; }
            //Gemi boyutu
            public Gemi(int boyut)
            {
                //Boyuta göre yeni gemi oluşturan yapıcı metot
                govde = new Nokta[boyut];
                for (int i = 0; i < boyut; i++)
                {
                    govde[i] = new Nokta();
                }
            }
            public Nokta Vuruyormu(Nokta atis)
            {
                foreach (Nokta nokta in govde)
                {
                    //Parametre olarak gelen atış noktası, gövdelerden biriyle eşit ise nokta nesnesi döndürülür
                    if (atis.x==nokta.x&&atis.y==nokta.y)
                    {
                        return nokta;
                    }
                }
                return null;
                //Null döner ise isabet etmemiştir
            }
        }
        class Oyuncu
        {
            //Oyuncu özellikleri
            public string isim { get; set; }
            public int skor { get; set; }
            //Oyuncunun gemileri
            public Gemi[] gemiler { get; set; }
            public int oyuncuNo { get; set; }
            public Oyuncu(string isim,int oyuncuNo)
            {
                //Yapıcı metot
                this.isim = isim;
                this.oyuncuNo = oyuncuNo;
                //Her oyuncu 3 gemiye sahip
                this.gemiler = new Gemi[3];
                this.skor = 0;
            }
            public bool gemiKaldimi()
            {
                //Oyuncunun vurulmayan gemisi kalmış ise oyun devam edecektir
                foreach (Gemi gemi in gemiler)
                {
                    //Her geminin gövdeleri kontrol edilir
                    foreach (Nokta nokta in gemi.govde)
                    {
                        if (!nokta.vuruldumu)
                        {
                            //Noktalardan en az bir tanesi vurulmadıysa oyun devam eder
                            return true;
                        }
                    }
                }
                return false;
            }
            private bool UstUstemi(int x)
            {
                //aynı satırda 2 gemi olamamsını sağlayacak UstUstemi metotu
                foreach (Gemi gemi in gemiler)
                {
                    //Her geminin gövdesinin x değeri kontrol edilir
                    if (gemi!=null)
                    {
                        //Gemi boş değil ise gövdesinin x değeri kontrol eidlir
                        if (gemi.govde[0].x == x)
                        {
                            //Aynı satırdalar ise true döner yani üst üstedir
                            return true;
                        }
                    }
                }
                return false;
            }
            public void gemiOlustur(OyunTahtasi tahta)
            {
                //2-5 arasında döngü döner yani 3'er adet gemi oluşturulur
                for (int i = 2; i < 5; i++)
                {
                    //yeni gemi nesnesi oluşturulur
                    gemiler[i - 2] = new Gemi(i);
                    int x;
                    do
                    {
                        //Oyuncu 1 ise tahtanın üstünde
                        //Oyuncu 2 ise tahtanın alt yarısında gemiler yerleştirilir
                        if (oyuncuNo == 1)
                        {
                            x = random.Next(0, tahta.boyut / 2);
                        }
                        else
                        {
                            x = random.Next(tahta.boyut / 2, tahta.boyut);
                        }
                    } while (UstUstemi(x));
                    //Ardından gemilerin gövdeleri oluşturulur
                    gemiler[i - 2].govde[0] = new Nokta(x, random.Next(0, tahta.boyut - i));
                    for (int j = 0; j < i; j++)
                    {
                        //Gemilerin gövdeleri sağa doğru yerleştirilir
                        gemiler[i - 2].govde[j] = new Nokta(x, gemiler[i - 2].govde[0].y + j);
                    }
                }
            }
            public void AtisYap(OyunTahtasi tahta,Nokta atis)
            {
                bool vurduMu = false;
                if (oyuncuNo==1)
                {//Oyuncu 1 ise
                    foreach (Gemi gemi in tahta.oyuncu2.gemiler)
                    {
                        //Atış noktaları oyuncunun her gemisinin her gövdesi için kontrol edilir
                        Nokta vuruyormu = gemi.Vuruyormu(atis);
                        if (vuruyormu != null)
                        {
                            foreach (Nokta govde in gemi.govde)
                            {
                                if (govde.x == vuruyormu.x && govde.y == vuruyormu.y && !govde.vuruldumu)
                                {
                                    //Eğer vurulmuşsa skor arttırılır ve gövdenin vuruldu mu değeri true olur
                                    govde.vuruldumu = true;
                                    this.skor++;
                                    vurduMu = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                { //Oyuncu 2 ise aynı işlemler oyuncu1'in gemiler için yapılır
                    foreach (Gemi gemi in tahta.oyuncu1.gemiler)
                    {
                        Nokta vuruyormu = gemi.Vuruyormu(atis);
                        if (vuruyormu != null)
                        {
                            foreach (Nokta govde in gemi.govde)
                            {
                                if (govde.x == vuruyormu.x && govde.y == vuruyormu.y&&!govde.vuruldumu)
                                {
                                    govde.vuruldumu = true;
                                    this.skor++;
                                    vurduMu = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (vurduMu)
                {
                    Console.WriteLine("İsabetli atış! Skor:" + skor);
                }
                else
                    Console.WriteLine("Karavana!");
            }
        }
        class OyunTahtasi
        {
            public int[,] Tahta { get; set; }
            public Oyuncu oyuncu1 { get; set; }
            public Oyuncu oyuncu2 { get; set; }
            public int boyut { get; set; }
            public OyunTahtasi(Oyuncu oyuncu1, Oyuncu oyuncu2,int boyut)
            {
                //Yapıcı metot
                this.oyuncu1 = oyuncu1;
                this.oyuncu2 = oyuncu2;
                this.boyut = boyut;
            }
            public void TahtaGuncelle()
            {
                Tahta = new int[boyut, boyut];
                for (int i = 0; i < boyut; i++)
                {
                    for (int j = 0; j < boyut; j++)
                    {
                        //Tahtanın tüm değerleri ilk 0 olarak işaretlenir
                        Tahta[i, j] = 0;
                    }
                }
                foreach (Gemi gemi in oyuncu1.gemiler)
                {
                    foreach (Nokta nokta in gemi.govde)
                    {
                        //Ardından vurulan oyuncu1 gemileri -1, vurulmayanlar 1 olarak işaretlenir
                        if (nokta.vuruldumu)
                        {
                            Tahta[nokta.x, nokta.y] = -1;
                        }
                        else
                        {
                            Tahta[nokta.x, nokta.y] = 1;
                        }
                    }
                }
                foreach (Gemi gemi in oyuncu2.gemiler)
                {
                    foreach (Nokta nokta in gemi.govde)
                    {
                        //Ardından vurulan oyuncu2 gemileri -2, vurulmayanlar 2 olarak işaretlenir
                        if (nokta.vuruldumu)
                        {
                            Tahta[nokta.x, nokta.y] = -2;
                        }
                        else
                        {
                            Tahta[nokta.x, nokta.y] = 2;
                        }
                    }
                }
            }
            public void tahtaYazdir(int oyuncuNo)
            {
                Console.Write("  ");
                for (int i = 0; i < boyut; i++)
                {
                    Console.Write(i + " ");
                }
                Console.WriteLine();
                for (int i = 0; i < boyut; i++)
                {
                    Console.Write(i + " ");
                    for (int j = 0; j < boyut; j++)
                    {
                        if (oyuncuNo==0)
                        {
                            //Eğer parametre 0 ise tahta gizlenmeden yazılır
                            Console.Write(Tahta[i, j] + " ");
                        }
                        else
                        {
                            if (Tahta[i, j] == oyuncuNo)
                            {
                                //eğer tahta rakip oyuncu numarası ise 0 ile gizlenir
                                Console.Write("0 ");
                            }
                            //Değilse değer aynı şekilde yazılır
                            else Console.Write(Tahta[i, j] + " ");
                        }
                    }
                    Console.WriteLine();
                }
            }
            public bool OyunBittimi()
            {
                //2 Oyuncudan birinin gemisi kalmadı ise oyun biter
                if (oyuncu1.gemiKaldimi() && oyuncu2.gemiKaldimi())
                {
                    return true;
                }
                else return false;
            }
        }
         static Nokta Oyna(Oyuncu oyuncu)
        {
            //Oyuncuların satır ve sütun değerlerini girip oynamalarını sağlayan metot
            //alınan x ve y değerlerini nokta nesnesi olarak döndürür
            int x, y;
            Console.WriteLine("Atış Konumu");
            Console.Write("Satır: ");
            x = int.Parse(Console.ReadLine());
            Console.Write("Sutun: ");
            y = int.Parse(Console.ReadLine());
            return new Nokta(x, y);
        }
        static void Main(string[] args)
        {
            Oyuncu oyuncu1 = new Oyuncu("Oyuncu 1", 1);
            Oyuncu oyuncu2 = new Oyuncu("Oyuncu 2", 2);
            OyunTahtasi oyunTahtasi = new OyunTahtasi(oyuncu1,oyuncu2,10);
            oyunTahtasi.oyuncu1.gemiOlustur(oyunTahtasi);
            oyunTahtasi.oyuncu2.gemiOlustur(oyunTahtasi);
            oyunTahtasi.TahtaGuncelle();
            //oyunTahtasi.tahtaYazdir(0);
            //Tahta yazdır 0 ise tamamı görünür
            //1 ise oyuncu 2'nin
            //2 ise oyuncu 1'in gemileri görünür
            while (oyunTahtasi.OyunBittimi())
            {
                //Oyun bitene kadar sırayla oyuncular hamle yapar
                oyunTahtasi.tahtaYazdir(2);
                Console.WriteLine(oyuncu1.isim + "'in sırası!");
                oyunTahtasi.oyuncu1.AtisYap(oyunTahtasi, Oyna(oyuncu1));
                oyunTahtasi.TahtaGuncelle();
                oyunTahtasi.tahtaYazdir(1);
                Console.WriteLine(oyuncu2.isim + "'in sırası!");
                oyunTahtasi.oyuncu2.AtisYap(oyunTahtasi, Oyna(oyuncu2));
                oyunTahtasi.TahtaGuncelle();
            }
            Console.WriteLine("Oyun bitti!");
            if (oyunTahtasi.oyuncu1.skor>oyunTahtasi.oyuncu2.skor)
            {
                //Hangi oyuncunun skoru yüksek ise kazandı bilgisi verilir
                Console.WriteLine("Oyuncu 1 Kazandı!");
            }
            else
            {
                Console.WriteLine("Oyuncu 2 Kazandı!");
            }
            Console.ReadKey();
        }
    }
}
