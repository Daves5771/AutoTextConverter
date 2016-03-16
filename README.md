# AutoTextConverter
Auto Text Converter Source Code
![Alt text](/ScreenShots/fig1.png?raw=true "Figure 1")
![Alt text](/ScreenShots/fig2.png?raw=true "Figure 2")
1.	Introduction
1.1	Purpose 
The Auto Text Converter is a powerful search-and-replace tool for Microsoft Word Documents.  
It utilizes regular expressions, a feature missing in standard MS Office installations, as the heart of its search-and-replace engine.
With this application one can accomplish such tasks as capturing all of the URLs in a document, converting words of British spelling 
into their American equivalents and vice-versa. The application comes with  number of pluggins for text validation and replacements.
New plugins can be written and added to the plugins directory.
The search and replace abilities are only limited by one’s imagination.
  
1.2	Key Features
This document is intended for developers 
•	Automated or manual controlled searching and or text replacements.
•	Exporting the list of searched entries to a text or EXCEL file.
•	The inclusion of several plugins which are used to extend the ability to perform sophisticated text replacements such as converting a date format, e.g., 3/17/2016 into March 17, 2016.
•	The ability of writing your own plugins and adding them to the application.
•	The ability of reviewing all the changes that were made during a search-and-replace session and choosing to either revert or commit that change.
•	One can instantly navigate to any search result by simply double-clicking the entry in the application list view.
•	And many more options.

1.3	Requirements
Before running the application please read, understand, and agree to the following:

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE SaelSoft or David Saelman BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
The following must be present on your PC in order to run the application:

•	Operating System: WIN XP SP3, VISTA, Windows 8, 8.1, 10

•	.NET 4.0

•	Microsoft WORD 2007, 2010, 2013, 2016

2.	Overview
Below is a picture of the main application GUI.
The tool bar shows the following function buttons (from left to right)
Open file
Save file
Close file
Operations COMBO box 
Auto process button
Pause button
Stop button
Step button
Clear the Listview button


3.	Operation
3.1	Running the Application
After starting the application, the user can choose one of several search profile XML files from the Operations combobox.  By going to the View->  After opening the file with the application, the user can press the <process> button and the application will search paragraph by paragraph for a match.  The processing can be paused or resumed by pressing the <pause> button.  Pressing the <stop> button will stop the processing. 


3.2	Running Modes
One can process the file in one of two mode: (1) Process mode, which will process the entire file automatically. (2) Step mode, which will stop at each change. The user then needs to hit the step button again and the program will proceed until the next change.  The user can change modes simply by pressing the <Pause> button and pressing the mode of their choice.

4.	Making Custom Searches and Replacements
The user can create his own XML profiles for custom searches and replacements.  In this version, the user must manually create the XML file.  Existing files can be used as a template or starting point.  Below is a description of the XML format :

4.1	Header block
Gives general information about the XML file including version, owner and a description. The only required field is the ProfileVersion which should be set to 1.0

4.2	Search sections
Gives general information about the XML file including version, owner and a description.

4.2.1	RegEx
The regular expression. Note: one should first test the regular expression using the handy regular expression tester available from the Tools Menu.

4.2.2	Identifier
This field can be used for a private description of the section.

4.2.3	FindColor
The color the application will use for the text that will be searched for or replaced.  The list of colors which can be used can be accessed from the Tools Color Guide menu. In the future, one will be able to use the auto keyword if they do not wish to change the color of the text.

4.2.4	Action
If the action is defined as “find”, the engine will only perform a search but no replacement will be made. If the Action = “replace”, the text will be replaced.

4.2.5	Plugin
If the section supports a plugin, Plugin Description =”yes”, otherwise <Plugin></Plugin>

4.2.5.1	PluginName 
The name of the Plugin Assembly

4.2.5.2	PluginValidationFunction (Optional)
The name of the Plugin function to validate the input string.  For example, a numerical sting can be checked if it is a valid social security number or credit card number.

4.2.5.3	PluginFormatFunction
The Plugin function name that formats the input.

4.2.5.4	Reserved
Currently not used.

4.2.6	Reserved1
Currently not used

4.2.7	Reserved2
Currently not used

4.2.8	Description
The description used to identify what the regular expression will look for.  The description appears in the RegExListBox available from the View menu.
4.3	XML Example
Gives general information about the XML file including version, owner and a description.

<Searches>
  <Header>
    <Product>Auto Text Converter by SaelSoft</Product>
    <ProfileOwner>David Saelman</ProfileOwner>
    <CreationDate>March 10, 2016</CreationDate>
    <ProfileVersion>1.0</ProfileVersion>
    <ProfileDescription>Converts dates and performs other numerical formatting</ProfileDescription>
    <ProfileHistory />
  </Header>
  <Search RegEx= "(0[1-9]|1[012])[-\u2013/.](0[1-9]|[12][0-9]|3[01])[-\u2013/.]((19|20)\d\d)">
    <Identifier>URL</Identifier>
    <FindColor>Teal</FindColor>
    <Action>replace</Action>
    <Plugin Description="yes">
      <PlugInName>NumericDateValidator.dll</PlugInName>
      <PlugInValidationFunction>ValidateUSDate</PlugInValidationFunction>
      <PlugInFormatFunction>GetDateMatch_1</PlugInFormatFunction>
      <Reserved>0</Reserved>
    </Plugin>    
    <Resrved1/>
    <Resrved2/>
    <Description Text= "01/02/2008 -> January 2, 2008"/>  
  </Search>
</Searches>


4.4	Header block
Gives general information about the XML file including version, owner and a description.


5.	Using the Examples
The Auto Text Converter packages comes with several sample documents that you can try along with the sample XML profiles.

