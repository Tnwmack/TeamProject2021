## 2021Summer_Team1Repo - Corpsite
* This uses Vue 3, Bootstrap 5 and .NET 5.
* This application acts as an internal organizational website for a company. This site has a home screen that contains the company name, company message and, the company chief executive. This site has an about us screen which has information pertaining to the company. There is a products screen which has a table which will allow users to enter new products to the table, delete products from the table and sort the table. There is also an employees screen which lists all of the employees the company has and has similar add, delete and sort functionality that the products table has. The site will have a personal profile screen which will display information about individual employees. The site will also have a product details screen which shows extended information about a product. 

##Development Environment and setup
* Visual Studio - https://visualstudio.microsoft.com/
* VS Code - https://code.visualstudio.com/
* Node - https://nodejs.org/en/
* Vue:
	* I added src/styles/main.scss for Bootstrap styles, note I changed the primary color to green.
	* I added a component 'Weather' that accesses the API weather demo. Go to the about page and click the green 'Get Weather' button to see the API data call.
* Install:
	* Install npm - npm i
	* Install Vue cli and yarn:
		* npm install -g @vue/cli
		* npm install -g yarn
* To run:
	* Open the API solution in VS and click 'IIS Express' at the top.
	* Open the web folder in a console and run 'yarn install' and then 'yarn serve' and open the link it prints.
