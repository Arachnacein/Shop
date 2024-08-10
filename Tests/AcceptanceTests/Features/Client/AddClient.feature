Feature: Dodawanie klienta
  As a user
  I want to add a new customer to the application
  So that the customer can be managed within the system

  Scenario: Dodanie nowego klienta
    Given użytkownik jest na stronie dodawania klienta
    When użytkownik wprowadza poprawne dane klienta
    And użytkownik zatwierdza dodanie klienta
    Then nowy klient jest dodany do bazy danych
