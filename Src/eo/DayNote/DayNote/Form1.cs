using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Management;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using EO.WebBrowser;
namespace DayNote
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private static int width = Screen.PrimaryScreen.Bounds.Width;
        private static int height = Screen.PrimaryScreen.Bounds.Height;
        public static Form1 form = (Form1)Application.OpenForms["Form1"];
        public static string txt, path, readText;
        private void Form1_Shown(object sender, EventArgs e)
        {
            this.pictureBox1.Dock = DockStyle.Fill;
            EO.WebEngine.BrowserOptions options = new EO.WebEngine.BrowserOptions();
            options.EnableWebSecurity = false;
            EO.WebBrowser.Runtime.DefaultEngineOptions.SetDefaultBrowserOptions(options);
            EO.WebEngine.Engine.Default.Options.AllowProprietaryMediaFormats();
            EO.WebEngine.Engine.Default.Options.SetDefaultBrowserOptions(new EO.WebEngine.BrowserOptions
            {
                EnableWebSecurity = false
            });
            this.webView1.Create(pictureBox1.Handle);
            this.webView1.Engine.Options.AllowProprietaryMediaFormats();
            this.webView1.SetOptions(new EO.WebEngine.BrowserOptions
            {
                EnableWebSecurity = false
            });
            this.webView1.Engine.Options.DisableGPU = false;
            this.webView1.Engine.Options.DisableSpellChecker = true;
            this.webView1.Engine.Options.CustomUserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
            path = @"ppia.html";
            readText = DecryptFiles(path + ".encrypted", "tybtrybrtyertu50727885");
            webView1.LoadHtml(readText);
            webView1.RegisterJSExtensionFunction("SaveStorage", new JSExtInvokeHandler(WebView_JSSaveStorage));
            webView1.RegisterJSExtensionFunction("OpenStorage", new JSExtInvokeHandler(WebView_JSOpenStorage));
            webView1.RegisterJSExtensionFunction("DownloadTXT", new JSExtInvokeHandler(WebView_JSDownloadTXT));
        }
        public static string DecryptFiles(string inputFile, string password)
        {
            using (var input = File.OpenRead(inputFile))
            {
                byte[] salt = new byte[8];
                input.Read(salt, 0, salt.Length);
                using (var decryptedStream = new MemoryStream())
                using (var pbkdf = new Rfc2898DeriveBytes(password, salt))
                using (var aes = new RijndaelManaged())
                using (var decryptor = aes.CreateDecryptor(pbkdf.GetBytes(aes.KeySize / 8), pbkdf.GetBytes(aes.BlockSize / 8)))
                using (var cs = new CryptoStream(input, decryptor, CryptoStreamMode.Read))
                {
                    string contents;
                    int data;
                    while ((data = cs.ReadByte()) != -1)
                        decryptedStream.WriteByte((byte)data);
                    decryptedStream.Position = 0;
                    using (StreamReader sr = new StreamReader(decryptedStream))
                        contents = sr.ReadToEnd();
                    decryptedStream.Flush();
                    return contents;
                }
            }
        }
        private void webView1_LoadCompleted(object sender, LoadCompletedEventArgs e)
        {
            Task.Run(() => LoadPage());
        }
        private void LoadPage()
        {
            string tempsavepath = System.Reflection.Assembly.GetEntryAssembly().Location.Replace(@"file:\", "").Replace(Process.GetCurrentProcess().ProcessName + ".exe", "").Replace(@"\", "/").Replace(@"//", "") + "tempsave";
            string savedstorage = "[]";
            if (File.Exists(tempsavepath))
            {
                using (StreamReader file = new StreamReader(tempsavepath))
                {
                    savedstorage = file.ReadLine().Replace(@"""", "'");
                }
            }
            else
            {
                using (StreamWriter createdfile = new StreamWriter(tempsavepath))
                {
                    createdfile.WriteLine("[]");
                }
            }
            string backgroundcolor = "";
            using (System.IO.StreamReader file = new System.IO.StreamReader("color.txt"))
            {
                file.ReadLine();
                backgroundcolor = file.ReadLine();
                file.Close();
            }
            string stringinject;
            stringinject = @"
    <style>
        body {
            font-family: sans-serif;
            background-color: backgroundcolor;
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

        tr, td {
            width: 6vh;
            height: 3vh;
        }

        table td:first-child {
            width: 20vh;
            height: 3vh;
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
            background-color: backgroundcolor;
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
            background-color: backgroundcolor;
        }

        .table-hover {
            margin-bottom: 0px;
        }

        td {
            color: #FFFFFF;
            background-color: backgroundcolor;
        }

        ::-webkit-scrollbar {
            width: 10px;
        }

        ::-webkit-scrollbar-track {
            background: backgroundcolor;
        }

        ::-webkit-scrollbar-thumb {
            background: #888;
        }
            ::-webkit-scrollbar-thumb:hover {
                background: #eee;
            }
    </style>
".Replace("\r\n", " ").Replace("backgroundcolor", backgroundcolor);
            stringinject = @"""" + stringinject + @"""";
            stringinject = @"$(" + stringinject + @" ).appendTo('head');";
            this.webView1.EvalScript(stringinject);
            stringinject = (@"<div class='icon-download'>
        <label for='filename'>
            <div class='bg-light foldersave'>
                <i class='fa fa-save'></i>
            </div>
        </label>
        <input type='button' onClick='handleFilename()' value='Save' class='button' id='filename'>
    </div>

    <div class='icon-upload'>
        <label for='txtFileInput'>
            <div class='bg-light folderopen'>
                <i class='fa fa-folder-open'></i>
            </div>
        </label>
        <input type='button' id='txtFileInput' onclick='OpenStorage()'>
    </div>

    <div class='year-container'>
        <div class='bg-light yearminus' onclick='yearminus();'>
            <i class='fa fa-minus'></i>
        </div>
        <div id='year'>
        </div>
        <div class='bg-light yearplus' onclick='yearplus();'>
            <i class='fa fa-plus'></i>
        </div>
    </div>

    <div class='mark-container'>
        daynote
    </div>

    <div class='menu-container'>
    </div>

    <div class='calendar-container' id='calendar'>
    </div>

    <div class='Note-container'>
    </div>

    <div class='fa fa-spinner fa-spin spinner hide'></div>

    <script>

var sizescreenx = $(document).width();
var sizescreeny = $(document).height();
var notes = {};

function changeTitle() {
    document.title = 'daynote by michael franiatte';
}

$(function() {
    changeTitle();
    var getitem = JSON.stringify(savedstorage);
    var actualyear = new Date().getFullYear();
    createCalendar(actualyear);
	createYear(actualyear);
});

function createYear(year) {
	document.getElementById('year').innerHTML = year;
}

function yearminus() {
	var yeartochange = document.getElementById('year').innerHTML;
	var nb = Number(yeartochange);
	nb--;
	year = nb.toString();
    createCalendar(year);
	createYear(year);
}

function yearplus() {
	var yeartochange = document.getElementById('year').innerHTML;
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
    var htmlString = '';
	document.getElementById('calendar').innerHTML = '';
	var table = document.createElement('table');
	var number = table.insertRow(-1);
	for (var j = 0; j <= 31; j++) {
		var firstNameCell = number.insertCell(-1);
		firstNameCell.style.textAlign = 'center';
		if (j != 0) {
			firstNameCell.appendChild(document.createTextNode(j.toString()));
		}
	}
	for (var i = 1; i <= 12; i++) {
		var day = table.insertRow(-1);
		for (var j = 0; j <= 31; j++) {
			var firstNameCell = day.insertCell(-1);
			firstNameCell.style.textAlign = 'center';
			if (j == 0) {
				firstNameCell.appendChild(document.createTextNode('Days'));
			}
			else {
				var eachdate = new Date(i.toString() + '/' + j.toString() + '/' + actualyear);
				firstNameCell.appendChild(document.createTextNode(dayWeek(eachdate, true)));
				if (isLastDay(eachdate)) {
					break;
				}
			}
		}
		var note = table.insertRow(-1);
		for (var j = 0; j <= 31; j++) {
			var firstNameCell = note.insertCell(-1);
			firstNameCell.style.textAlign = 'center';
			firstNameCell.id = i.toString() + '/' + j.toString() + '/' + actualyear;
			if (j == 0) {
				firstNameCell.style.fontWeight = 'bolder';
				if (i == 1) {
					firstNameCell.appendChild(document.createTextNode('January'));
				}
				if (i == 2) {
					firstNameCell.appendChild(document.createTextNode('February'));
				}
				if (i == 3) {
					firstNameCell.appendChild(document.createTextNode('March'));
				}
				if (i == 4) {
					firstNameCell.appendChild(document.createTextNode('April'));
				}
				if (i == 5) {
					firstNameCell.appendChild(document.createTextNode('May'));
				}
				if (i == 6) {
					firstNameCell.appendChild(document.createTextNode('June'));
				}
				if (i == 7) {
					firstNameCell.appendChild(document.createTextNode('Jully'));
				}
				if (i == 8) {
					firstNameCell.appendChild(document.createTextNode('August'));
				}
				if (i == 9) {
					firstNameCell.appendChild(document.createTextNode('September'));
				}
				if (i == 10) {
					firstNameCell.appendChild(document.createTextNode('October'));
				}
				if (i == 11) {
					firstNameCell.appendChild(document.createTextNode('November'));
				}
				if (i == 12) {
					firstNameCell.appendChild(document.createTextNode('December'));
				}
			}
			if (j != 0) {
				firstNameCell.addEventListener('click', function(){
					showNote(this.id);
				});
				firstNameCell.className = 'cursorpointer';
				var eachdate = new Date(i.toString() + '/' + j.toString() + '/' + actualyear);
				if (isLastDay(eachdate)) {
					break;
				}
			}
		}
	}
	document.getElementById('calendar').appendChild(table);
	createNoteEditor(actualyear);
}

function dayWeek(eachdate, abreviation) { 
	var dayoftheweek = eachdate.getDay();
	if (dayoftheweek == 0) {
		if (abreviation)
			return 'Su';
		else
			return 'Sunday';
	}
	if (dayoftheweek == 1) {
		if (abreviation)
			return 'Mo';
		else
			return 'Monday';
	}
	if (dayoftheweek == 2) {
		if (abreviation)
			return 'Tu';
		else
			return 'Tuesday';
	}
	if (dayoftheweek == 3) {
		if (abreviation)
			return 'We';
		else
			return 'Wednesday';
	}
	if (dayoftheweek == 4) {
		if (abreviation)
			return 'Th';
		else
			return 'Thursday';
	}
	if (dayoftheweek == 5) {
		if (abreviation)
			return 'Fr';
		else
			return 'Friday';
	}
	if (dayoftheweek == 6) {
		if (abreviation)
			return 'Sa';
		else
			return 'Saturday';
	}
}

function showNote(id) {
    var overlayid = 'overlay-' + id;
	var noteeditor = document.getElementById(overlayid);
	noteeditor.style.display = 'block';
}

function handleFilename() {
	exportTableToTXT('daynote.txt');
}

function exportTableToTXT(filename) {
    var txt = JSON.stringify(savedstorage);
    DownloadTXT(txt, filename);
}

function createNoteEditor(actualyear) {
    $('.spinner').removeClass('hide');
    $('.spinner').addClass('show');
    var notecases = document.getElementsByClassName('cursorpointer');
    for (var i = 0; i < notecases.length; i++) {
		var id = 'overlay-' + notecases[i].id;
		var htmlString = '';
		htmlString = `<div class=\'noteeditors\' id=` + id + `>
						<div class=\'bg-light close\' style=\'display:float;position:absolute;float:right;right:1%;top:1%;\' onclick=\'editorClose();\'>
						<i class=\'fa fa-window-close\' style=\'color:#FFFFFF;\'></i></div>
					  </div>`;
		notecases[i].insertAdjacentHTML('beforeend', htmlString);
    }
	for (var i = 0; i < notecases.length; i++) {
		var eachdate = new Date(notecases[i].id);
		var id = 'overlay-' + notecases[i].id;
		var notecase = document.getElementById(id);
		var noteid = 'note-' + notecases[i].id;
		var htmlString = '';
		htmlString = `
			<div class=\'table-hover-outer\'>
				<table class=\'table table-hover\' id=` + noteid + ` contenteditable=\'true\' style=\'width:100%;height:100%;\'>
					<thead>
						<th>Note ` + dayWeek(eachdate, false) + ` ` + notecases[i].id + `</th>
					</thead>
					<tbody>
						<tr>
							<td></td>
						</tr>
					</tbody>
				</table>
			</div>`;
		htmlString += '';
		notecase.insertAdjacentHTML('beforeend', htmlString);
		htmlString = `<button type=\'button\' id=\'btn_add_note\' onclick=\'addNote()\'>Add Note</button>
					  <button type=\'button\' id=\'btn_save_note\' onclick=\'saveNote()\'>Save Note</button>`;
		notecase.insertAdjacentHTML('beforeend', htmlString);
	}
    try {
		notes = JSON.parse(JSON.stringify(savedstorage) || '[]');
		notes = transformObj(notes);
		var grouped = transformArr(notes);
		grouped.forEach(function(val, index) {
			var d = new Date(val.date);
			var n = d.getFullYear();
			if (n == actualyear) {
				var date = val.date;
				var cells = val.notes;
				var id = 'note-' + val.date;
				var tbl = document.getElementById(id);
				var i = 0;
				var j= 0;
				for (let cell of cells) {
					if (cell.note != '' & cell.note != null & cell.note != undefined & cell.note != '<br>') {
						if (i == 0) {
							tbl.rows[1].deleteCell(0);
							var x = tbl.rows[1].insertCell(0);
							x.innerHTML = decodeURIComponent(escape(window.atob(cell.note)));
						}
						else {
							var x = tbl.insertRow();
							var y = x.insertCell(0);
							y.innerHTML = decodeURIComponent(escape(window.atob(cell.note)));
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
    catch { }
    $('.spinner').removeClass('show');
    $('.spinner').addClass('hide');
}

function editorClose() {
	setTimeout(function () {
		var elems = document.getElementsByClassName('noteeditors');
		for (var i = 0; i < elems.length; i++) {
			elems[i].style.display = 'none';
		}
	}
	, 100);
}

function addNote(){
    var id = $('.table-hover:visible').attr('id'); 
	var tbl = document.getElementById(id);
    var counter = 1;
    var numberOfCols = tbl.rows[0].cells.length;
    var row = tbl.insertRow();
    for (var i = 0; i < numberOfCols; i++) {
        var cell = row.insertCell(i);
        cell.id = 'row' + (tbl.rows.length - 1) + 'cell' + counter;
        counter++;
    }
}

function saveNote(){
    $('.spinner').removeClass('hide');
    $('.spinner').addClass('show');
	newnotes = [];
    notes = JSON.parse(JSON.stringify(savedstorage) || '[]');
    notes = transformObj(notes);
    var id = $('.table-hover:visible').attr('id'); 
	var date = id.replace('note-', '');
    notes.forEach(function(val, index) {
		if (val.date != date & val.note != '' & val.note != null & val.note != undefined & val.note != '<br>') {
			newnotes.push({note: val.note, date: val.date});
		}
    });
	var tbl = document.getElementById(id);
    var numberofines = tbl.rows.length;
    for (var i = 1; i < numberofines; i++) {
        var cell = tbl.rows[i].cells[0].innerHTML;
		if (cell != '' & cell != null & cell != undefined & cell != '<br>') {
            cell = window.btoa(unescape(encodeURIComponent(cell)));
			newnotes.push({note: cell, date: date});
		}
    }
    var tempgrouper = newnotes;
    var grouped = transformArr(tempgrouper);
    grouped = transformInpand(grouped);
	SaveStorage(JSON.stringify(grouped));
}

function transformInpand(orig) {
    var grouped = [];
    orig.forEach(function(val, index) {
        var name = val.date;
        var files = val.notes;
        var tempgrouped = [];
        for (let file of files) {
            var note = file.note;
            tempgrouped.push(note);
         };
        grouped.push({notes : tempgrouped, date: name});
    });
    return grouped;
}

function transformObj(orig) {
    notes = [];
    orig.forEach(function(val, index) {
        var name = val.date;
        var files = val.notes;
        for (let file of files) {
             notes.push({note: file, date: name});
         };
    });
    return notes;
}

function transformArr(orig) {
    var newArr = [], notes = {}, i, j, cur;
    for (i = 0, j = orig.length; i < j; i++) {
        cur = orig[i];
        if (!(cur.date in notes)) {
            notes[cur.date] = {date: cur.date, notes: []};
            newArr.push(notes[cur.date]);
        }
        notes[cur.date].notes.push({note: cur.note});
    }
    return newArr;
}
</script>
<script>

document.body.addEventListener('mousedown', clicked, false);

function clicked(e) {
    if (e.button == 2) {
        var source = window.getSelection().toString();
        if (source != '' & source != null & source != 'undefined') {
            var input = document.createElement('textarea');
            input.value = source;
            document.body.appendChild(input);
            input.select();
            document.execCommand('Copy');
            input.remove();
        }
    }
}

$('body').on('click', 'img', function() {
    var source = this.src;
    var input = document.createElement('textarea');
    input.value = source;
    document.body.appendChild(input);
    input.select();
    document.execCommand('Copy');
    input.remove();
});

$('body').on('click', 'a', function() {
    var source = this.href;
    var input = document.createElement('textarea');
    input.value = source;
    document.body.appendChild(input);
    input.select();
    document.execCommand('Copy');
    input.remove();
});

</script>
").Replace("\r\n", " ").Replace("savedstorage", savedstorage);
            stringinject = @"""" + stringinject + @"""";
            stringinject = @"$(document).ready(function(){$('body').append(" + stringinject + @");});";
            this.webView1.EvalScript(stringinject);
        }
        void WebView_JSSaveStorage(object sender, JSExtInvokeArgs e)
        {
            string tempsavepath = System.Reflection.Assembly.GetEntryAssembly().Location.Replace(@"file:\", "").Replace(Process.GetCurrentProcess().ProcessName + ".exe", "").Replace(@"\", "/").Replace(@"//", "") + "tempsave";
            using (StreamWriter createdfile = new StreamWriter(tempsavepath))
            {
                string str = e.Arguments[0] as string;
                createdfile.WriteLine(str);
            }
            webView1.LoadHtml(readText);
        }
        [STAThread]
        void WebView_JSOpenStorage(object sender, JSExtInvokeArgs e)
        {
            Thread newThread = new Thread(new ThreadStart(showOpenFileDialog));
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
        }
        public void showOpenFileDialog()
        {
            string str = "";
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "All Files(*.*)|*.*";
            if (op.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader file = new StreamReader(op.FileName))
                {
                    str = file.ReadLine();
                }
                string tempsavepath = System.Reflection.Assembly.GetEntryAssembly().Location.Replace(@"file:\", "").Replace(Process.GetCurrentProcess().ProcessName + ".exe", "").Replace(@"\", "/").Replace(@"//", "") + "tempsave";
                using (StreamWriter createdfile = new StreamWriter(tempsavepath))
                {
                    createdfile.WriteLine(str);
                }
                webView1.LoadHtml(readText);
            }
        }
        [STAThread]
        void WebView_JSDownloadTXT(object sender, JSExtInvokeArgs e)
        {
            txt = e.Arguments[0] as string;
            Thread newThread = new Thread(new ThreadStart(showSaveFileAsDialog));
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
        }
        public void showSaveFileAsDialog()
        {
            SaveFileDialog sa = new SaveFileDialog();
            sa.Filter = "All Files(*.*)|*.*";
            if (sa.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter createdfile = new StreamWriter(sa.FileName))
                {
                    createdfile.WriteLine(txt);
                }
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.webView1.Dispose();
        }
    }
}
