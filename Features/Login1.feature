Feature: FarmersLogin
	Login to FarmersFz

  @Farmers_home_page
  Scenario: Check the Farmers Home page
    Given I visit "app.site.url"
    When I enter username in the "app.username" field
    And I enter password in the "app.password" field
    And I press the "login" button
    Then I should see the Farmers Home page