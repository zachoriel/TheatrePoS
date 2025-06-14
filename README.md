<b>Movie Theatre Point of Sale System.</b>

<b>Background</b>: I worked at a small-town, family-owned movie theater from 2017 - 2020. One day, without any warning or clear cause, our point of sale system completely stopped functioning. We later discovered that the company who owned the software went out of business and shut down all of their infrastructure. This put us in a pretty big bind; while it was possible to do everything by hand, it was a massive inconvenience. So I went home that evening and immediately got to work on this: a backup point of sale system, crafted specifically to visually represent the old system as closely as possible for minimum employee adjustment. The next morning, I published a release build here, and downloaded it on all our computers (with the owner's permission). The software wasn't perfect; I had no way of facilitating credit/debit card transfers through the app on such short notice, but it eliminated the need to take orders and track sales/inventory by hand, and provided some logging that even our old system didn't have. For the time that we used this, I liked to joke that we were probably the only movie theater in the world whose business was powered by Unity. 

Just in case it's a concern, I received permission (enthusiastically!) from the owners to use this project on my resume. 

<b>Features</b>:

- User account creation & login authentication
     - Option to delete accounts
- Grid of buttons for each concessions item
     - Special button for inputting custom prices
- Numbered shopping cart list
     - Option to remove items
- Shift summary displaying logged in user, numbered list of all items sold, & total profits
- Savable shift summaries stored in a text file in a unique desktop folder
     - Folder structure: Shift Summaries -> mm-dd-yyyy -> "Shift Summary - hh-mm-ss".txt
 

<b>NOTE:</b> I know the sales text is wonky. This was made in 1 night in the middle of a crisis situation. 😅

<b>Sales Window</b>:
![Screenshot (457)](https://user-images.githubusercontent.com/31017086/74197141-a3895f00-4c13-11ea-8854-084c819e755d.png)


<b>Shift Summary Window</b>:
![Screenshot (458)](https://user-images.githubusercontent.com/31017086/74197156-aab06d00-4c13-11ea-85fb-acd40d15f2a9.png)
