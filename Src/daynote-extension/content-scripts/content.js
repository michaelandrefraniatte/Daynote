chrome.runtime.onMessage.addListener(function(request, sender, sendResponse) {

document.getElementsByTagName('html')[0].innerHTML = '<head></head><body></body>';

var script = document.createElement('script'); 
script.src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js'; 
document.head.appendChild(script);

var stringinject = `
    <link rel="shortcut icon" href="${request.favicon}" type="image/png" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.css">
    <style>

body {
    font-family: sans-serif;
    background-color: #222222;
    color: #FFFFFF;
    font-size: 2.65vh;
}

.slideshow-container {
    justify-content: center;
    display: flex;
}

.menushow-container {
    justify-content: center;
    display: flex;
    color: #FFFFFF;
}

.hide {
    display: none;
}

.show {
    display: flex;
}

.spinner {
    position: absolute;
    top: calc(50% - 10vh);
    left: calc(50% - 10vh);
    text-align: center;
    font-size: 20vh;
}

table {
    border-collapse: collapse;
}

table, th, td {
    border: 0.1vh solid white;
}

.calendar-container tbody tr td {
    height: 3vh;
    width: 7vh;
}

.noteeditors tbody tr td {
    width: 20vh;
    height: 6vh;
}

.cursorpointer {
    cursor: pointer;
    font-weight: bolder;
}

.mark-container {
    position: absolute;
    right: 1%;
    top: 1vh;
}

.calendar-container {
    position: absolute;
    top: 5vh;
}

#year {
    position: absolute;
    left: 48.6%;
    top: 1vh;
    text-decoration: underline;
    text-align: center;
    font-weight: bolder;
}

.yearplus {
    cursor: pointer;
    position: absolute;
    right: 46%;
    top: 1vh;
}

.yearminus {
    cursor: pointer;
    position: absolute;
    left: 46%;
    top: 1vh;
}

.foldersave {
    cursor: pointer;
    position: absolute;
    left: 1%;
    top: 1vh;
}

.folderopen {
    cursor: pointer;
    position: absolute;
    left: 3%;
    top: 1vh;
}

.icon-upload > input {
    display: none;
}

.icon-upload > input {
    display: none;
}

.icon-download > input {
    display: none;
}

.noteeditors {
    background-color: #222222;
    position: absolute;
    width: 50%;
    height: 50%;
    left: 25%;
    top: 25%;
    display: none;
}

.displayed {
    display: block;
}

.undisplayed {
    display: none;
}

tbody {
    height: 100%;
    width: 100%;
    min-width: 100%;
    display: block;
    overflow-y: auto;
}

.table-hover-outer {
    height: 92%;
    width: 100%;
    overflow-x: auto;
    white-space: nowrap;
}

button {
    background-color: #222222;
}

.table-hover {
    margin-bottom:0px;
}

td {
    color: #FFFFFF;
    background-color:#222222;
}

/* width */
::-webkit-scrollbar {
    width: 10px;
}

/* Track */
::-webkit-scrollbar-track {
    background: #000;
}

/* Handle */
::-webkit-scrollbar-thumb {
    background: #888;
}

    /* Handle on hover */
    ::-webkit-scrollbar-thumb:hover {
        background: #eee;
    }

    </style>`;

document.getElementsByTagName('head')[0].innerHTML = stringinject;

stringinject = `

    <!-- Foldersave container -->
    <div class="icon-download">
        <label for="filename">
            <div class="bg-light foldersave">
                <i class="fa fa-save"></i>
            </div>
        </label>
        <input type="button" onClick="handleFilename()" value="Save" class="button" id="filename">
    </div>

    <!-- Folderopen container -->
    <div class="icon-upload">
        <label for="txtFileInput">
            <div class="bg-light folderopen">
                <i class="fa fa-folder-open"></i>
            </div>
        </label>
        <input type="file" id="txtFileInput" onchange="handleFiles(this.files)" accept=".txt">
    </div>

    <!-- Year container -->
    <div class="year-container">
        <div class="bg-light yearminus" onclick="yearminus();">
            <i class="fa fa-minus"></i>
        </div>
        <div id="year">
        </div>
        <div class="bg-light yearplus" onclick="yearplus();">
            <i class="fa fa-plus"></i>
        </div>
    </div>

    <!-- Mark container -->
    <div class="mark-container">
        daynote
    </div>

    <!-- Menu container -->
    <div class="menu-container">
    </div>

    <!-- Calendar container -->
    <div class="calendar-container" id="calendar">
    </div>

    <!-- Note container -->
    <div class="Note-container">
    </div>

    <!-- Loading spinner overlay container -->
    <div class="fa fa-spinner fa-spin spinner hide"></div>

    <script>


var sizescreenx = $(document).width();
var sizescreeny = $(document).height();
var notes = {};

function changeTitle() {
    document.title = "daynote";
}

$(function() {
    changeTitle();
    var getitem = localStorage.getItem("notes");
    if (getitem == "" | getitem == null | getitem == "undefined") {
        localStorage.setItem("notes", "[]");
    }
    var actualyear = new Date().getFullYear();
    createCalendar(actualyear);
	createYear(actualyear);
});

function reloadCalendar() {
    $(".mark-container").empty();
    $(".menu-container").empty();
    $(".calendar-container").empty();
    $(".Note-container").empty();
    var getitem = localStorage.getItem("notes");
    if (getitem == "" | getitem == null | getitem == "undefined") {
        localStorage.setItem("notes", "[]");
    }
    var actualyear = new Date().getFullYear();
    createCalendar(actualyear);
	createYear(actualyear);
}

function createYear(year) {
	document.getElementById("year").innerHTML = year;
}

function yearminus() {
	var yeartochange = document.getElementById("year").innerHTML;
	var nb = Number(yeartochange);
	nb--;
	year = nb.toString();
    createCalendar(year);
	createYear(year);
}

function yearplus() {
	var yeartochange = document.getElementById("year").innerHTML;
	var nb = Number(yeartochange);
	nb++;
	year = nb.toString();
    createCalendar(year);
	createYear(year);
}

function isLastDay(dt) {
    var test = new Date(dt.getTime());
    test.setDate(test.getDate() + 1);
    return test.getDate() === 1;
}

function createCalendar(actualyear) {
    var htmlString = "";
    console.log(htmlString);
	document.getElementById("calendar").innerHTML = "";
	var table = document.createElement("table");
	var number = table.insertRow(-1);
	for (var j = 0; j <= 31; j++) {
		var firstNameCell = number.insertCell(-1);
		firstNameCell.style.textAlign = "center";
		if (j != 0) {
			firstNameCell.appendChild(document.createTextNode(j.toString()));
		}
	}
	for (var i = 1; i <= 12; i++) {
		var day = table.insertRow(-1);
		for (var j = 0; j <= 31; j++) {
			var firstNameCell = day.insertCell(-1);
			firstNameCell.style.textAlign = "center";
			if (j == 0) {
				firstNameCell.appendChild(document.createTextNode("Days"));
			}
			else {
				var eachdate = new Date(i.toString() + "/" + j.toString() + "/" + actualyear);
				firstNameCell.appendChild(document.createTextNode(dayWeek(eachdate, true)));
				if (isLastDay(eachdate)) {
					break;
				}
			}
		}
		var note = table.insertRow(-1);
		for (var j = 0; j <= 31; j++) {
			var firstNameCell = note.insertCell(-1);
			firstNameCell.style.textAlign = "center";
			firstNameCell.id = i.toString() + "/" + j.toString() + "/" + actualyear;
			if (j == 0) {
				firstNameCell.style.fontWeight = "bolder";
				if (i == 1) {
					firstNameCell.appendChild(document.createTextNode("January"));
				}
				if (i == 2) {
					firstNameCell.appendChild(document.createTextNode("February"));
				}
				if (i == 3) {
					firstNameCell.appendChild(document.createTextNode("March"));
				}
				if (i == 4) {
					firstNameCell.appendChild(document.createTextNode("April"));
				}
				if (i == 5) {
					firstNameCell.appendChild(document.createTextNode("May"));
				}
				if (i == 6) {
					firstNameCell.appendChild(document.createTextNode("June"));
				}
				if (i == 7) {
					firstNameCell.appendChild(document.createTextNode("Jully"));
				}
				if (i == 8) {
					firstNameCell.appendChild(document.createTextNode("August"));
				}
				if (i == 9) {
					firstNameCell.appendChild(document.createTextNode("September"));
				}
				if (i == 10) {
					firstNameCell.appendChild(document.createTextNode("October"));
				}
				if (i == 11) {
					firstNameCell.appendChild(document.createTextNode("November"));
				}
				if (i == 12) {
					firstNameCell.appendChild(document.createTextNode("December"));
				}
			}
			if (j != 0) {
				firstNameCell.addEventListener("click", function(){
					showNote(this.id);
				});
				firstNameCell.className = "cursorpointer";
				var eachdate = new Date(i.toString() + "/" + j.toString() + "/" + actualyear);
				if (isLastDay(eachdate)) {
					break;
				}
			}
		}
	}
	document.getElementById("calendar").appendChild(table);
	createNoteEditor(actualyear);
}

function dayWeek(eachdate, abreviation) {
	var dayoftheweek = eachdate.getDay();
	if (dayoftheweek == 0) {
		if (abreviation)
			return "Su";
		else
			return "Sunday";
	}
	if (dayoftheweek == 1) {
		if (abreviation)
			return "Mo";
		else
			return "Monday";
	}
	if (dayoftheweek == 2) {
		if (abreviation)
			return "Tu";
		else
			return "Tuesday";
	}
	if (dayoftheweek == 3) {
		if (abreviation)
			return "We";
		else
			return "Wednesday";
	}
	if (dayoftheweek == 4) {
		if (abreviation)
			return "Th";
		else
			return "Thursday";
	}
	if (dayoftheweek == 5) {
		if (abreviation)
			return "Fr";
		else
			return "Friday";
	}
	if (dayoftheweek == 6) {
		if (abreviation)
			return "Sa";
		else
			return "Saturday";
	}
}

function showNote(id) {
    var overlayid = "overlay-" + id;
	var noteeditor = document.getElementById(overlayid);
	noteeditor.style.display = "block";
}

function handleFilename() {
	exportTableToTXT("daynote.txt");
}

function exportTableToTXT(filename) {
    var txt = localStorage.getItem("notes");
    downloadTXT(txt, filename);
}

function downloadTXT(txt, filename) {
    var txtFile;
    var downloadLink;
	if (window.Blob == undefined || window.URL == undefined || window.URL.createObjectURL == undefined) {
		alert("Your browser does not support Blobs");
		return;
	}
    txtFile = new Blob([txt], {type:"text/txt"});
    downloadLink = document.createElement("a");
    downloadLink.download = filename;
    downloadLink.href = window.URL.createObjectURL(txtFile);
    downloadLink.style.display = "none";
    document.body.appendChild(downloadLink);
    downloadLink.click();
}

$(function() {
    $("#txtFileInput").click(function(){
        $(this).val("");
    });
});

function handleFiles(files) {
    $(".spinner").removeClass("hide");
    $(".spinner").addClass("show");
	getAsText(files[0]);
}

function getAsText(fileToRead) {
	var reader = new FileReader();
	reader.onload = loadHandler;
	reader.onerror = errorHandler;
	reader.readAsText(fileToRead);
}

function loadHandler(event) {
	var txt = event.target.result;
	processData(txt);
}

function errorHandler(evt) {
	if(evt.target.error.name == "NotReadableError") {
		alert("Can not read file !");
	}
}

function processData(txt) {
    localStorage.setItem("notes", txt);
    reloadCalendar();
}

function createNoteEditor(actualyear) {
    $(".spinner").removeClass("hide");
    $(".spinner").addClass("show");
    var notecases = document.getElementsByClassName("cursorpointer");
    for (var i = 0; i < notecases.length; i++) {
		var id = "overlay-" + notecases[i].id;
		var htmlString = "";
		htmlString = "<div class=\'noteeditors\' id=" + id + "><div class=\'bg-light close\' style=\'display:float;position:absolute;float:right;right:1%;top:1%;\' onclick=\'editorClose();\'><i class=\'fa fa-window-close\' style=\'color:#FFFFFF;\'></i></div></div >";
		notecases[i].insertAdjacentHTML("beforeend", htmlString);
    }
	for (var i = 0; i < notecases.length; i++) {
		var eachdate = new Date(notecases[i].id);
		var id = "overlay-" + notecases[i].id;
		var notecase = document.getElementById(id);
		var noteid = "note-" + notecases[i].id;
		var htmlString = "";
		htmlString = "<div class=\'table-hover-outer\'><table class=\'table table-hover\' id=" + noteid + " contenteditable=\'true\' style=\'width:100%;height:100%;\'><thead><th>Note " + dayWeek(eachdate, false) + " " + notecases[i].id + "</th></thead ><tbody><tr><td></td></tr></tbody></table></div >";
		htmlString += "";
		notecase.insertAdjacentHTML("beforeend", htmlString);
		htmlString = "<button type =\'button\' id=\'btn_add_note\' onclick=\'addNote()\'>Add Note</button><button type =\'button\' id=\'btn_save_note\' onclick=\'saveNote()\'>Save Note</button>";
        notecase.insertAdjacentHTML("beforeend", htmlString);
}
    try {
    notes = JSON.parse(localStorage.getItem("notes") || "[]");
    notes = transformObj(notes);
    var grouped = transformArr(notes);
    console.log(grouped);
    grouped.forEach(function (val, index) {
        var d = new Date(val.date);
        var n = d.getFullYear();
        if (n == actualyear) {
            var date = val.date;
            var cells = val.notes;
            var id = "note-" + val.date;
            var tbl = document.getElementById(id);
            var i = 0;
            var j = 0;
            for (let cell of cells) {
                if (cell.note != "" & cell.note != null & cell.note != undefined & cell.note != "<br>") {
                    if (i == 0) {
                        tbl.rows[1].deleteCell(0);
                        var x = tbl.rows[1].insertCell(0);
                        x.innerHTML = cell.note;
                    }
                    else {
                        var x = tbl.insertRow();
                        var y = x.insertCell(0)
                        y.innerHTML = cell.note;
                    }
                    j++;
                }
                i++;
            }
            if (j > 0) {
                var caseday = document.getElementById(date);
                var text = document.createTextNode(j.toString());
                caseday.appendChild(text);
            }
        }
    });
}
catch {
    localStorage.setItem("notes", "[]");
}
$(".spinner").removeClass("show");
$(".spinner").addClass("hide");
}

function editorClose() {
    setTimeout(function () {
        var elems = document.getElementsByClassName("noteeditors");
        for (var i = 0; i < elems.length; i++) {
            elems[i].style.display = "none";
        }
    }
        , 100);
}

function addNote() {
    var id = $(".table-hover:visible").attr("id");
    var tbl = document.getElementById(id);
    var counter = 1;
    var numberOfCols = tbl.rows[0].cells.length;
    var row = tbl.insertRow();
    for (var i = 0; i < numberOfCols; i++) {
        var cell = row.insertCell(i);
        cell.id = "row" + (tbl.rows.length - 1) + "cell" + counter;
        counter++;
    }
}

function saveNote() {
    $(".spinner").removeClass("hide");
    $(".spinner").addClass("show");
    newnotes = [];
    notes = JSON.parse(localStorage.getItem("notes") || "[]");
    notes = transformObj(notes);
    var id = $(".table-hover:visible").attr("id");
    var date = id.replace("note-", "");
    notes.forEach(function (val, index) {
        if (val.date != date & val.note != "" & val.note != null & val.note != undefined & val.note != "<br>") {
            newnotes.push({ note: val.note, date: val.date });
        }
    });
    var tbl = document.getElementById(id);
    var numberofines = tbl.rows.length;
    for (var i = 1; i < numberofines; i++) {
        var cell = tbl.rows[i].cells[0].innerHTML;
        if (cell != "" & cell != null & cell != undefined & cell != "<br>") {
            newnotes.push({ note: cell, date: date });
        }
    }
    var tempgrouper = newnotes;
    var grouped = transformArr(tempgrouper);
    grouped = transformInpand(grouped);
    localStorage.setItem("notes", JSON.stringify(grouped));
    reloadCalendar();
}

function transformInpand(orig) {
    var grouped = [];
    orig.forEach(function (val, index) {
        var name = val.date;
        var files = val.notes;
        var tempgrouped = [];
        for (let file of files) {
            var note = file.note;
            tempgrouped.push(note);
        };
        grouped.push({ notes: tempgrouped, date: name });
    });
    return grouped;
}

function transformObj(orig) {
    notes = [];
    orig.forEach(function (val, index) {
        var name = val.date;
        var files = val.notes;
        for (let file of files) {
            notes.push({ note: file, date: name });
        };
    });
    return notes;
}

function transformArr(orig) {
    var newArr = [], notes = {}, i, j, cur;
    for (i = 0, j = orig.length; i < j; i++) {
        cur = orig[i];
        if (!(cur.date in notes)) {
            notes[cur.date] = { date: cur.date, notes: [] };
            newArr.push(notes[cur.date]);
        }
        notes[cur.date].notes.push({ note: cur.note });
    }
    return newArr;
}

</script>`;

    (function () {
        // more or less stolen form jquery core and adapted by paul irish
        function getScript(url, success) {
            var script = document.createElement('script');
            script.src = url;
            var head = document.getElementsByTagName('head')[0],
                done = false;
            // Attach handlers for all browsers
            script.onload = script.onreadystatechange = function () {
                if (!done && (!this.readyState
                    || this.readyState == 'loaded'
                    || this.readyState == 'complete')) {
                    done = true;
                    success();
                    script.onload = script.onreadystatechange = null;
                    head.removeChild(script);
                }
            };
            head.appendChild(script);
        }
        getScript('https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js', function () {
            $(document).ready(function () {
                var script = document.createElement('script');
                script.src = 'https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js';
                document.head.appendChild(script);
                $('body').html(stringinject);
            });
        });
    })();

  sendResponse({ fromcontent: "This message is from content.js" });
});
