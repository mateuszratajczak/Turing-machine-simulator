/*
 * Created by SharpDevelop.
 * User: Monika Sikora
 * Date: 13.12.2017
 * Time: 23:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace Turing
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		int aktualna_pozycja_glowicy = 0;   //określa aktualną pozycję głowicy maszyny Turinga - zmienna globalna używana w Akcjach związanych z przyciskami i w main()
		int dl_liczby = 0;					//długość ciągu znaków pobranego z pliku, pozwala wstawić odpowiednią liczbę TextBoxów i CheckBoxów (dynamicznie) i poruszać się po nich 
		int ktora_liczba_w_pliku = 0;		//ile liczb jest w naszym pliku
		string[] kolejne_liczby;			//tablica z rozdzielonymi liczbami z pliku
		string liczba;
		
		public void Elementy()										//funkcja wstawiająca na formatkę Textboxy i Checkboxy i umieszczajaca w nich odowiednie dla liczby cyfry
		{
			if(ktora_liczba_w_pliku < (kolejne_liczby.Length - 1)) 	//odrzucamy liczbę pustą po ostatnim znaku #
			{
				stan = 1;											//stan początkowy 
				aktualna_pozycja_glowicy = 0;						//na początku głowica pokazuje na zerowy element tablicy
				
				for(int i = 0; i < dl_liczby; i++)					//uruchamia się tylko gdy istanieją już textboxy i chexboxy na formatce a my chcemy uruchomić program dla kolejnej liczby
				{
					string elem2 = "tb"+i.ToString();				//nazwa TextBoxa u mnie składa się z tb i numerka odpowiadającego elementom liczby, czyli w liczbie -123 pod 0 kryje się "-" itd.
					
					TextBox t_box_to_remove = this.Controls.Find(elem2, true).FirstOrDefault() as TextBox; //wyszukiwanie takiego TextBoxa i przypisanie zmiennej t_box_to_remove referencji do  obiektu o takiej nazwie
					
					this.Controls.Remove(t_box_to_remove); 			//usunięcie tego elemntu z formatki
					
					string elem3 = "chb"+i.ToString();				//podobne zastosowanie dla CheckBoxów na formatce
					CheckBox ch_box_to_remove = this.Controls.Find(elem3, true).FirstOrDefault() as CheckBox;
					this.Controls.Remove(ch_box_to_remove);
				}	

				liczba = kolejne_liczby[ktora_liczba_w_pliku]; 	//pobranie z wczytanej w main tablicy kojelne_liczby liczby do przetwarzania
				liczba = " " + liczba + " "; 							// aby to dobrze wyglądało przed naszą liczbą i po dodaję spację (przydatne gdy do 99 dodajemy 3 wtedy mamy 102 a po liczbie poneważ głowica musi "wyjść" za liczbę
				dl_liczby = liczba.Length;   							//liczymy sobie długość tej liczby
				
				//label4 = new Label();
				//label4.Text = 
				
			//---------------------- Dynamiczne wstawianie TextBoxow -----------------------------------------
				TextBox t_box;
				for (int i = 0; i < dl_liczby; i++) 					//wstawiamy tyle TextBoxów ile długość liczby
				{				
				    t_box = new TextBox();
				    t_box.Name = "tb" + i.ToString(); 					//nadajemy nazwę tb - textbox , chb - checkbox i jego pozycji
			    
				    if(i < dl_liczby)									// tu wstawiamy cyfry do stworzonych TextBoxów (i te spacje na początku i końcu) 
				    {
				    	char liczba_na_pozycji = liczba[i]; 			//zmienna pomocnicza w której jest chwilowo nasza cyfra
				    	t_box.Text = liczba_na_pozycji.ToString();		//dodanie cyfry do TextBoxa
				    }
			    
				    t_box.AutoSize = false; 							//wielkość TextBoxa
				    t_box.ReadOnly = true;  							//TextBox dostępny tylko do odczytu
				    t_box.Width = 30;									//szerokość Textboxa
				    t_box.TextAlign = HorizontalAlignment.Center; 		//położenie centralne cyfry w TextBoxie
			  												
				    t_box.Location = new Point(i * 50, 10); 			//lokacja nowego  TextBoxa na formatce
				    													//Point(poziomo, pionowo)
				    this.Controls.Add(t_box);							//dodanie elementu na formatkę
				}
			//---------------------- Dynamiczne wstawianie CheckBoxow ---------------------------------------
				CheckBox box;
				for (int i = 0; i < dl_liczby; i++) 					//pętla wstawiająca CheckBoxy na tej samej zasadzie co TextBoxy
				{
				    box = new CheckBox();
				    box.Name = "chb" + i.ToString();
				   	box.Enabled = false;
				    box.AutoSize = true;
				    box.Width = 30;
															   
					box.Location = new Point(8 + (i * 50), 40); 		//8px pozwala wyśrodkować checkboxa względem Textboxa
			    	this.Controls.Add(box);
				}
            
        	    	string elem = "chb"+aktualna_pozycja_glowicy.ToString(); 						//zmienna pomocnicza konkatenująca chb i numer aktualnego położenia głowicy aby go zaznaczyć
 					CheckBox mon = this.Controls.Find(elem, true).FirstOrDefault() as CheckBox; 	//wyszukiwanie Checkboxa o tej nazwie
					mon.Checked = true;																//zaznaczenie tego checkboxa
				
			}//if
			else 												//uruchamia się gdy program zakończy pracę. Oba przyciski są nieaktywne. 
			{
				button2.Enabled = false;
				button1.Enabled = false;
				MessageBox.Show("Program zakończył pracę");		//Komunikat o końcu pracy
				stan = 11;										//Stan 11. Oba przyciski nieaktywne. 
				//button1.Enabled = false;
			}

		}//Elementy()
		
		
		public MainForm()
		{
			
			string zaw_pliku = System.IO.File.ReadAllText("turing.txt");	//odczyt z pliku całego textu do stringu zaw_pliku
			kolejne_liczby = zaw_pliku.Split('#');							//rozdzielenie tekstu w pliku na elementu rozdzielone '#' - zapisanie do tablicy kolejne liczby
			
			Elementy(); 													//uruchomienie Metody Elememnty, która utworzy  TextBoxy i Checkboxy + wczyta do nich liczbę
			
			InitializeComponent();
			
		}
		int stan = 1; 														//deklaracja globalnej zmiennej stan - początkowo przyjmuje wartość 1.
		
		void Button1Click(object sender, EventArgs e)						//Event po kliknięciu pzycisku "Przesuń głowicę"
		{
			label4.Text = liczba;
			//Stany są odworotnej kolejności niż w Maszynie Turinga. Dlaczego? Ponieważ stan 1 zmienia się na np. stan 3 i wtedy
			//gdy będzie on pod nim w jednym kroku wykonają się dwie intukcje i zobaczymy rezultat tej drugiej bo pierwsza się 
			//wykona ale będzie niezauważalnie szybko wykonana. Teraz gdy np. stan 1 zmieni się na stan 3 istukcje dla stanu 3
			//zostaną wykonane dopiero po ponownym naciśnięciu przycisku "Przesuń głowice"		
			
			button2.Enabled = false;			//gdy przetwarzamy liczbę maszyną Turinga nie możemy przełączyć się do kolejnej jeśli ta nie będzie skończona
			
			if(stan == 8) 						//specyficzny stan dla stanu Q8 wg tablicy przejść czyli on sprawdza czy przed zerem po zmianie jest minus czy liczba jak minus czyli do -3 dodaliśmy 3 i mamy -0 to wtedy usuwa ten minus
			{
				label2.Text = "Q8";
				
				string elem = "chb"+aktualna_pozycja_glowicy.ToString(); 	//wyszukianie obecnie zaznaczonego elementu checkbox
				CheckBox mon = this.Controls.Find(elem, true).FirstOrDefault() as CheckBox;
				mon.Checked = false;										//i odznaczenie go
			
			aktualna_pozycja_glowicy-=1;									//przesunięcie głowicy w prawo
			
				elem = "chb"+aktualna_pozycja_glowicy.ToString();			//wyszukanie checkboxa dla tego położenia głowicy
				mon = this.Controls.Find(elem, true).FirstOrDefault() as CheckBox;
				mon.Checked = true;											//i zaznaczenie go
				
				string elem2 = "tb"+aktualna_pozycja_glowicy.ToString();	//teraz patrzymy co jest w tym Textboxie dla tego położenia głowicy
				TextBox wartosc = this.Controls.Find(elem2, true).FirstOrDefault() as TextBox;
            	string k = wartosc.Text;									//zczytanie wartości z tego TextBoxa 
            	
            	if(k != "-")
            		stan = 10;			//gdy element przed 0 po zamianie jest liczbą
           
            	if(k == "-")			//sytuacja dla -0 likwiduje wtedy ten minus.
            	{
            		wartosc.Text = " ";
            		stan = 10;
            	}
            	
            	
			}//if(stan == 8)
			
			
			
			
			//---------------------------------- cofanie się po Turingu gdy -3, -2, -1  -------------------------
			if(stan == 7) 						//specyficzny stan dla liczb ujemnych  -2, -1 bo wtedy po dodaniu 3 mamy 
			{
				label2.Text = "Q7";
				
				string elem = "chb"+aktualna_pozycja_glowicy.ToString(); 	//wyszukianie obecnie zaznaczonego elementu checkbox
				CheckBox mon = this.Controls.Find(elem, true).FirstOrDefault() as CheckBox;
				mon.Checked = false;										//i odznaczenie go
			
			aktualna_pozycja_glowicy+=1;									//przesunięcie głowicy w prawo
			
				elem = "chb"+aktualna_pozycja_glowicy.ToString();			//wyszukanie checkboxa dla tego położenia głowicy
				mon = this.Controls.Find(elem, true).FirstOrDefault() as CheckBox;
				mon.Checked = true;											//i zaznaczenie go
				
				string elem2 = "tb"+aktualna_pozycja_glowicy.ToString();	//teraz patrzymy co jest w tym Textboxie dla tego położenia głowicy
				TextBox wartosc = this.Controls.Find(elem2, true).FirstOrDefault() as TextBox;
            	string k = wartosc.Text;									//zczytanie wartości z tego TextBoxa 
            	
            	if(k == "8")
            	{
            		wartosc.Text = "2";  //według tablicy przejść realizacja przekszałcania dla -1 i -2.
            		stan = 10;
            	}
            	if(k == "9")
            	{
            		wartosc.Text = "1";
            		stan = 10;
            	}
            	
            	
			}//if(stan == 7)
			
			
			//---------------------------------- cofanie się po Turingu odejmowanie 1 -------------------------
			//stan gdy mamy taką systuację -321 wtedy musimy osobno przetworzyć 1 na 8 ale to wykonuje stan 5. a ten stan odejmie 2 - 1 i wpisze jeden.
			if(stan == 6)
			{
				label2.Text = "Q6";
				
				string elem = "chb"+aktualna_pozycja_glowicy.ToString(); 				//przesuwanie głowicy czyli checkboxów
				CheckBox mon = this.Controls.Find(elem, true).FirstOrDefault() as CheckBox;
				mon.Checked = false;
			
			aktualna_pozycja_glowicy-=1; 												//odejmujemy 1 od pozycji
			
				elem = "chb"+aktualna_pozycja_glowicy.ToString();
				mon = this.Controls.Find(elem, true).FirstOrDefault() as CheckBox;
				mon.Checked = true;
				
				string elem2 = "tb"+aktualna_pozycja_glowicy.ToString();				//zczytanie wartości textboxa z aktualnej pozycji głowicy
				TextBox wartosc = this.Controls.Find(elem2, true).FirstOrDefault() as TextBox;
            	string k = wartosc.Text;
				
            	if(k != " " && k!="-")						//gdy różny od spacji lub "-"
            	{
            		int cyfra = Int32.Parse(k);				//konwersja stringa na liczbę (cyfrę)
            		if(cyfra > 0)							//jeżeli większe od 0 to ...
	            	{
	            		cyfra -= 1;							//odejmij jedynkę, tak jak w przykładzie
	            		wartosc.Text = cyfra.ToString();	//wstaw wartość
	            		stan = 10;							//zakończ program
	            	}
            		else
            		{
            			wartosc.Text = "9";					//gdy tu będzie 0 to mamy sytację np. -101. Wtedy 1 zamieni się na 8 w if, ale gdy znajdziemy 0 
            												//to zastępujemy 9 i dajej musimy "zjeść" gdzieś w przodzie tą jedynkę bo specyficzną sytuację 
            												//-3, -2, -1 mamy zaprogramowaną
            			stan = 6;
            		}
            	}
            	if(k == "-")
            	{
            		wartosc.Text = " ";
            		stan = 7;
            	}
            	
			}//if(stan == 6)
			
			
			//---------------------------------- cofanie się po Turingu gdy minus  -------------------------
			//czyli tak na prawdę teraz będziemy odejmować tą trójkę gdy cyfry >= 3 w innym wypadku przejdziemy do stanu 5.
			if(stan == 5)
			{
				label2.Text = "Q5";
				string elem = "chb"+aktualna_pozycja_glowicy.ToString();				//wyszukanie zaznaczonego checkboxa i odznaczenie go
				CheckBox mon = this.Controls.Find(elem, true).FirstOrDefault() as CheckBox;
				mon.Checked = false;
			
			aktualna_pozycja_glowicy-=1;									//głowica w lewo
			
				elem = "chb"+aktualna_pozycja_glowicy.ToString();			//zaznaczenie nowej pozycji głowicy
				mon = this.Controls.Find(elem, true).FirstOrDefault() as CheckBox;
				mon.Checked = true;
				
				string elem2 = "tb"+aktualna_pozycja_glowicy.ToString();	//zczytanie co jest na tej pozycji w Textboxie
				TextBox wartosc = this.Controls.Find(elem2, true).FirstOrDefault() as TextBox;
            	string k = wartosc.Text;
				
            	int cyfra = Int32.Parse(k);									//konwersja na int
            	
            	if(cyfra > 2)						//gdy tak na prawdę ostatnia cyfra jest > 2 to odejmujemy normalnie 3
            	{
            		if(cyfra == 3)
            		{
            			cyfra -= 3;
            			wartosc.Text = cyfra.ToString();
            			stan = 8;
            		}
            		else
            		{
            			cyfra -= 3;
            			wartosc.Text = cyfra.ToString();
            			stan = 10;						//i kończymy program dla tej liczby
            		}
            		
            	}
            	else
            	{
            		cyfra -= 3;						//w przeciwnym razie np. -102 od 2 odejmujemu 3 i mamy -1 
            		cyfra = 10 + cyfra;				// -1 + 10 = 9
            		wartosc.Text = cyfra.ToString();	//i tą 9 wpisujemy i musimy zdjąć jeszcze tą jedynkę z przodu którą tu pożyczyliśmy więc stan 5.
            		stan = 6;
            	}
            	
			}//if(stan == 5)
			
			//-------------------------------- idziemy w prawo gdy minus z przodu -> ---------------------
			if(stan == 4 && aktualna_pozycja_glowicy< dl_liczby-1) 
			{
				label2.Text = "Q4";
				
				string elem = "chb"+aktualna_pozycja_glowicy.ToString();
				CheckBox mon = this.Controls.Find(elem, true).FirstOrDefault() as CheckBox;
				mon.Checked = false;
			
			aktualna_pozycja_glowicy+=1;
			
				elem = "chb"+aktualna_pozycja_glowicy.ToString();
				mon = this.Controls.Find(elem, true).FirstOrDefault() as CheckBox;
				mon.Checked = true;
				
			if(aktualna_pozycja_glowicy == dl_liczby-1)   //to jest przejście gdy liczba jest dodatnia - wtedy uruchamiamy stan 2
				stan=5;
			} //if(stan == 4)
			
			//---------------------------------- cofanie się po Turingu dodawanie 1 czyli to przeniesienie -------------------------
			if(stan == 3)
			{
				
				label2.Text = "Q3";
				//odczytujemy bieżący głowicy 
				string elem = "chb"+aktualna_pozycja_glowicy.ToString();
				CheckBox mon = this.Controls.Find(elem, true).FirstOrDefault() as CheckBox;
				mon.Checked = false;
			
			aktualna_pozycja_glowicy-=1; 									//idziemy w lewo
			
				elem = "chb"+aktualna_pozycja_glowicy.ToString();
				mon = this.Controls.Find(elem, true).FirstOrDefault() as CheckBox;
				mon.Checked = true;
				
				string elem2 = "tb"+aktualna_pozycja_glowicy.ToString();	//zczytujemy co za cyfra jest w bieżącym położeniu	
				TextBox wartosc = this.Controls.Find(elem2, true).FirstOrDefault() as TextBox;
            	string k = wartosc.Text;
				
            	if(k != " ")							//jeśli ten znak ('cyfra') jest różna od " " - zrobione dla bezpieczeństwa
            	{
            		int cyfra = Int32.Parse(k);			//wyciągamy jaka to cyfra 
            		if(cyfra < 9)
	            	{
	            		cyfra += 1;						//chcemy dodać to przeniesienie
	            		wartosc.Text = cyfra.ToString();
	            		stan = 10;						//jak się uda to kończymy program
	            	}
            		else
            		{
            			wartosc.Text = "0";				//gdy 9 to wpisz zero i idź dalej (tą jedynkę dopisuje się potem w else)
            		}
            	}
            	else									//jak będzie liczba 99 + 3 = 102 to tutaj właśnie jest dopisana ta jedynka w puste miejsce przed liczbą
            	{
            		wartosc.Text = "1";
            		stan = 10;
            	}
            	
			}//if(stan == 3)
			
			
			//---------------------------------- cofanie się po Turingu dodawanie 3  -------------------------
			if(stan == 2)
			{
				label2.Text = "Q2";
				string elem = "chb"+aktualna_pozycja_glowicy.ToString();
				CheckBox mon = this.Controls.Find(elem, true).FirstOrDefault() as CheckBox;
				mon.Checked = false;
			
			aktualna_pozycja_glowicy-=1;
			
				elem = "chb"+aktualna_pozycja_glowicy.ToString();
				mon = this.Controls.Find(elem, true).FirstOrDefault() as CheckBox;
				mon.Checked = true;
				
				string elem2 = "tb"+aktualna_pozycja_glowicy.ToString();
				TextBox wartosc = this.Controls.Find(elem2, true).FirstOrDefault() as TextBox;
            	string k = wartosc.Text;
				
            	int cyfra = Int32.Parse(k);
            	
            	if(cyfra < 7)			//jeśli cyfra jest mniejsza od 7 to bez problemu możemy dodać 3 i skończyć program
            	{
            		cyfra += 3;
            		wartosc.Text = cyfra.ToString();
            		
            		stan = 10;
            	}
            	else
            	{						//w przeciwnym razie dopisujemy resztę z liczby po dodaniu do niej 3 podzieloną przez 10.
            		cyfra += 3;
            		cyfra = cyfra % 10;
            		wartosc.Text = cyfra.ToString();
            		stan = 3;
            		
            	}
         
			}//if(stan == 2)
			
			//----------------------------- przejscie po liczbie - STAN POCZĄTKOWY --------------
			if(stan == 1 && aktualna_pozycja_glowicy< dl_liczby-1) //dopóki nie dojdziemy do końca liczby 
			{
				
			
				label2.Text = "Q1";
				
				string elem = "chb"+aktualna_pozycja_glowicy.ToString();
				CheckBox mon = this.Controls.Find(elem, true).FirstOrDefault() as CheckBox;
				mon.Checked = false;
			
			aktualna_pozycja_glowicy+=1;
			
				elem = "chb"+aktualna_pozycja_glowicy.ToString();
				mon = this.Controls.Find(elem, true).FirstOrDefault() as CheckBox;
				mon.Checked = true;
				
			if(aktualna_pozycja_glowicy == dl_liczby-1)   //to jest przejście gdy liczba jest dodatnia - wtedy uruchamiamy stan 2
				stan=2;
			
			string elem2 = "tb1"; //sprawdzamy czy jest minus (0 - " ", 1 - "-" albo cyfra ... patrząc na tablicę)
				TextBox wartosc = this.Controls.Find(elem2, true).FirstOrDefault() as TextBox;
            	string k = wartosc.Text;
            	
            string elem3 = "tb2"; //zczytujemy jaka liczba jest na 2 pozycji (używane gdy jest minus)
            TextBox wartosc2 = this.Controls.Find(elem3, true).FirstOrDefault() as TextBox;
            string licz = wartosc2.Text;
            
            
            
				
				
            	if(k == "-" ) //gdy mamy minus z przodu
            		stan = 4;											//uruchamiamy stan 4
            	
				//if(k == "-" && dl_liczby == 4  && aktualna_pozycja_glowicy == dl_liczby-1) //jest minus, dodatkowo jest to liczba np. " -2 ";
				//{
				//	int l_help = Int32.Parse(licz); //jeśli ta cyfra jest mniejsza od 4 to specjalny stan obsługujący -3, -2, -1
				//	if(l_help < 4)
				//		stan = 6;
				//}
            		
			}//stan == 1
			
			if(stan == 10)		//stan końca liczby - można przejść do następnej 
			{
				button1.Enabled = false; 
				button2.Enabled = true;
			}
			
			if(stan == 11)		//zakończenie programu - nie ma więcej liczb
			{
				button1.Enabled = false;
				button2.Enabled = false;
			}
			
		}
		
		void Button2Click(object sender, EventArgs e) //pobieranie kolejnej liczby do przetwarzania
		{
			label4.Text = "";
			label2.Text = "";
			ktora_liczba_w_pliku += 1; 			//przesuwamy index w tablicy kolejne_liczby
			button1.Enabled = false;	
			Elementy(); 						//stworzenie wyglądu formatki dla tej liczby
			if(ktora_liczba_w_pliku < (kolejne_liczby.Length - 1)) //jeśli jest jeszcze jakaś liczba to możemy ją przetwarzać
				button1.Enabled = true;						//więc przycisk przesuń głowicę jest aktywny
			
		}
		
		void Label2Click(object sender, EventArgs e)
		{
			
		}
	}
}
