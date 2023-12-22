using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;
using Microsoft.Web.WebView2.WinForms;
using System.Runtime.InteropServices;
using Microsoft.Web.WebView2.Wpf;
using WebView2 = Microsoft.Web.WebView2.WinForms.WebView2;
using System.Diagnostics;
using System.Management;
using System.Security.Cryptography;
using System.Net.Http;
using System.Security.Policy;

namespace DayNote
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static WebView2 webView21 = new WebView2(); 
        public static string filepath;
        public static string txt, path, readText;
        private async void Form1_Shown(object sender, EventArgs e)
        {
            CoreWebView2EnvironmentOptions options = new CoreWebView2EnvironmentOptions("--disable-web-security --autoplay-policy=no-user-gesture-required", "en");
            CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(null, null, options);
            await webView21.EnsureCoreWebView2Async(environment);
            webView21.CoreWebView2.SetVirtualHostNameToFolderMapping("appassets", "assets", CoreWebView2HostResourceAccessKind.DenyCors);
            filepath = @"file:///" + System.Reflection.Assembly.GetEntryAssembly().Location.Replace("\\", "/").Replace("DayNote.exe", "") + "assets/index.html";
            webView21.Source = new System.Uri(filepath);
            webView21.CoreWebView2.Settings.AreDevToolsEnabled = true;
            webView21.CoreWebView2.AddHostObjectToScript("bridge", new Bridge());
            webView21.NavigationStarting += WebView21_NavigationStarting;
            webView21.NavigationCompleted += WebView21_NavigationCompleted;
            webView21.LocationChanged += WebView21_LocationChanged;
            webView21.Dock = DockStyle.Fill;
            this.Controls.Add(webView21);
        }
        private void WebView21_LocationChanged(object sender, EventArgs e)
        {
        }
        private void WebView21_NavigationStarting(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
        }
        private void webView21_SourceChanged(object sender, Microsoft.Web.WebView2.Core.CoreWebView2SourceChangedEventArgs e)
        {
        }
        private async void WebView21_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
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
    <link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css'>
    <link rel='stylesheet' href='https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css'>
    <link rel='stylesheet' href='https://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.css'>
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
            stringinject = @"document.getElementsByTagName('head')[0].innerHTML = " + stringinject + @";";
            await execScriptHelper(stringinject);
            string stringcontent = @" 
    <div class='icon-download'>
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
        <input type='button' id='txtFileInput'>
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

const bridge = chrome.webview.hostObjects.bridge;

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
    var txt = JSON.stringify(savedstorage);
    bridge.DownloadTXT(txt);
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

async function saveNote(){
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
	await bridge.SaveStorage(JSON.stringify(grouped));
    location.reload();
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

document.getElementById('txtFileInput').onclick = async () => {
    await bridge.OpenStorage('');
    location.reload();
};

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
".Replace("\r\n", " ").Replace("savedstorage", savedstorage);
            stringcontent = @"""" + stringcontent + @"""";
            stringinject = @"(function () {
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
        if (typeof jQuery == 'undefined') {
            console.log('Sorry, but jQuery wasn\'t able to load');
        } else {
            console.log('This page is now jQuerified with v' + $.fn.jquery);
            $(document).ready(function () { });
                var script = document.createElement('script'); script.src = 'https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js'; document.head.appendChild(script);
                $(document).ready(function(){$('body').html(stringcontent);
            });
        }
    });
})();".Replace("stringcontent", stringcontent);
            await execScriptHelper(stringinject);
        }
        private async System.Threading.Tasks.Task<String> execScriptHelper(String script)
        {
            var x = await webView21.ExecuteScriptAsync(script).ConfigureAwait(false);
            return x;
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            webView21.Dispose();
        }
    }
    public static class Extensions
    {
        public static async Task<string> ExecuteScriptFunctionAsync(this WebView2 webView2, string functionName, params object[] parameters)
        {
            string script = functionName + "(";
            for (int i = 0; i < parameters.Length; i++)
            {
                script += JsonConvert.SerializeObject(parameters[i]);
                if (i < parameters.Length - 1)
                {
                    script += ", ";
                }
            }
            script += ");";
            return await webView2.ExecuteScriptAsync(script);
        }
    }
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class Bridge
    {
        public static Form1 form1 = new Form1();
        public static string txt;
        public string SaveStorage(string param)
        {
            string tempsavepath = System.Reflection.Assembly.GetEntryAssembly().Location.Replace(@"file:\", "").Replace(Process.GetCurrentProcess().ProcessName + ".exe", "").Replace(@"\", "/").Replace(@"//", "") + "tempsave";
            using (StreamWriter createdfile = new StreamWriter(tempsavepath))
            {
                string str = param;
                createdfile.WriteLine(str);
            }
            return param;
        }
        public string OpenStorage(string param)
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
            }
            return param;
        }
        public string DownloadTXT(string param)
        {
            txt = param;
            SaveFileDialog sa = new SaveFileDialog();
            sa.Filter = "All Files(*.*)|*.*";
            if (sa.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter createdfile = new StreamWriter(sa.FileName))
                {
                    createdfile.WriteLine(txt);
                }
            }
            return param;
        }
    }
}