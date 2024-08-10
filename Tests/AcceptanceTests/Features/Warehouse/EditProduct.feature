Feature: Edytowanie produktu
  Jako użytkownik
  Chcę edytować istniejący produkt w systemie
  Aby móc zaktualizować jego dane

  Scenario: Edytowanie istniejącego produktu
    Given użytkownik jest na stronie edytowania produktu
    And istnieje produkt nazwany "Laptop"
    When użytkownik wprowadza nowe dane produktu
    And użytkownik zatwierdza edytowanie produktu
    Then zmienione dane produktu są zapisane w bazie danych
