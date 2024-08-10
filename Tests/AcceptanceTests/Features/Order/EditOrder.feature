Feature: Edytowanie zamówienia
  Jako użytkownik
  Chcę edytować istniejące zamówienie
  Aby móc zmieniać jego szczegóły

  Scenario: Edytowanie istniejącego zamówienia
    Given istnieje zamówienie o id "1"
    When użytkownik edytuje zamówienie z Id "1" zmieniając ilość na "5" i cenę na "9999.95"
    Then zamówienie o Id "1" ma ilość "5"
    And zamówienie o Id "1" ma cenę "9999.95"
    And zamówienie o Id "1" ma status "nieukończone"
