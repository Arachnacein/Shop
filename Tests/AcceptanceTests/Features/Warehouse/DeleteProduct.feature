Feature: Usuwanie produktu
  Jako użytkownik
  Chcę usunąć istniejący produkt z systemu
  Aby usunąć produkty, które nie są już potrzebne

  Scenario: Usuwanie istniejącego produktu
    Given użytkownik jest na stronie usuwania produktu
    And istnieje produkt o nazwie "Laptop"
    When użytkownik zatwierdza usunięcie produktu
    Then produkt jest usunięty z bazy danych
    And produkt nie jest już dostępny w systemie
