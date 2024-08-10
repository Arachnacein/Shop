Feature: Usuwanie klienta
  Jako użytkownik
  Chcę usunąć istniejącego klienta z systemu
  Aby móc usunąć niepotrzebne lub nieaktualne dane

  Scenario: Pomyślne usunięcie istniejącego klienta
    Given klient do usunięcia istnieje w systemie
    When użytkownik usuwa klienta
    Then klient jest usunięty z bazy danych
