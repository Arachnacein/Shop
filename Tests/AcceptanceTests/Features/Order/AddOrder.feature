Feature: Dodawanie zamówienia
  Jako użytkownik
  Chcę dodać nowe zamówienie do systemu
  Aby móc zarządzać zamówieniami w systemie

  Scenario: Dodanie nowego zamówienia
    Given użytkownik jest na stronie dodawania zamówienia
    And istnieje produkt o Id "1"
    And istnieje użytkownik o Id "00000000-0000-0000-0000-000000000001"
    When użytkownik wprowadza poprawne dane zamówienia
    And użytkownik zatwierdza dodanie zamówienia
    Then nowe zamówienie jest dodane do bazy danych
    And zamówienie ma status nieukończone
