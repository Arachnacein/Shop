Feature: Oznaczanie zamówienia jako ukończone
  Jako użytkownik
  Chcę oznaczyć zamówienie jako ukończone
  Aby móc zarządzać statusem zamówień

  Scenario: Oznaczanie istniejącego zamówienia jako ukończone
    Given istnieje zamówienie o Id "1" z nieukończonym statusem
    When użytkownik oznacza zamówienie o Id "1" jako ukończone
    Then zamówienie o Id "1" ma status ukończone
