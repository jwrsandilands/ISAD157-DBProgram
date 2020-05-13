# ISAD157-DBProgram
The ISAD157 Sample Code Application

OVERVIEW
This Sample Code attempts to represent how a program that displays the information stored inside of the "FaceBook database" 
provided might be displayed to an admin, or someone higher up. This way it allows the admin to browse through user information
and links, and allows the to search and sort the information they find.

START-UP & USE INSTRUCTIONS
 - When the application starts, it will immediately search and pull the sample database. This may take a few seconds.
 - Once the form window has appeared, it will display all of the information. From here the user may sort or search.
 - Once the user has searched they are free to browse, perform another search, or sort.
 - In order to revert the application back to its original "Show All" state, either leave the searchbar blank when performing a
search, or hit the "Display All" button.

SEARCHING
 - Once a search is performed, it will attempt to find the relevant data to one's search across all of the data tables. This way
a user may compare all of the relevant information across all tables and the same time, how ever is required.
 - Different searches have different restrictions in how effective they can be. The most effective search is a "User ID" search.
This will return all information from a single user, granted the admin puts in the correct User ID number.
 - A "Messages > Date" search will automatically pull all of the messages between the input range from midnight on both specified 
Dates. Keep in mind, the specific search input for this search is "YYYY-MM-DD,YYYY-MM-DD".
 - One can search in any column in any table at any time. The admin does not need to pull all of the original data back up to 
search.

SORTING
 - The tables can be sorted by multiple columns at once. Say one wanted both of the user's Firstnames and Surnames to be ordered
as one scrolls through the table, this could be achieved by ticking both corrosponding "Sort By" boxes and hitting "Sort!".
 - The sorting method can be switched between an "ascending" and "descending" function. This way the admin can sort as such for
simpler browsing.
 - Sorts do not stay if one searches or resets the data tables. Keep in mind the Admin will have to re-apply the sort.
