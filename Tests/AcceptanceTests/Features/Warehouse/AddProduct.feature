Feature: Dodawanie produktu
  Jako użytkownik
  Chcę dodać nowy produkt do systemu
  Aby móc zarządzać nim w systemie

  Scenario: Dodanie nowego produktu
    Given użytkownik jest na stronie dodawania produktu
    When użytkownik wprowadza poprawne dane produktu
    And użytkownik zatwierdza dodanie produktu
    Then nowy produkt jest dodany do bazy danych
