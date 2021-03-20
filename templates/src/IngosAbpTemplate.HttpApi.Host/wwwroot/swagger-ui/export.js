window.addEventListener("load",
	function () {
		customizeSwaggerUI()
	});
function customizeSwaggerUI() {
	setTimeout(function () {
		const tag = '<rapi-pdf style="display:none" id="doc"></rapi-pdf>';
		const btn = '<button id="btn" style="font-size:16px;padding: 6px 16px;text-align: center;white-space: nowrap;background-color: orange;color: white;border: 0px solid #333;cursor: pointer;" type="button" onclick="download()">Download API Document</button>';
		const html = document.getElementsByClassName("info")[0].innerHTML;
		document.getElementsByClassName("info")[0].innerHTML = html + tag + btn
	},
		1200)
}
function download() {
	var client = new XMLHttpRequest();
	client.overrideMimeType("application/json");
	client.open("GET", "v1/swagger.json");
	var jsonApi = "";
	client.onreadystatechange = function () {
		if (client.responseText !== "undefined" && client.responseText !== "") {
			jsonApi = client.responseText;
			if (jsonApi !== "") {
				const doc = document.getElementById("doc");
				const key = jsonApi.replace('\"Authorization: Bearer {token}\"', "");
				const objSpec = JSON.parse(key);
				doc.generatePdf(objSpec)
			}
		}
	};
	client.send()
}