Feature: Usuwanie zamówienia
  Jako użytkownik
  Chcę usunąć istniejące zamówienie
  Aby móc zarządzać swoimi zamówieniami

  Scenario: Usuwanie istniejącego zamówienia
    Given istnieje zamówienie o Id "1"
    When użytkownik usuwa zamówienie o Id "1"
    Then zamówienie o Id "1" nie istnieje w bazie danych
