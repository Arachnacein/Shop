Feature: Edytowanie klienta
  Jako użytkownik
  Chcę edytować dane istniejącego klienta w systemie
  Aby móc zaktualizować nieprawidłowe lub nieaktualne informacje

  Scenario: Pomyślne edytowanie danych istniejącego klienta
    Given klient istnieje w systemie
    When użytkownik zmienia imię klienta na "Marek" i nazwisko na "Nowak"
    And zapisuje zmiany
    Then dane klienta są zaktualizowane w bazie danych
