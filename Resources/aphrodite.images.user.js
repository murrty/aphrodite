// ==UserScript==
// @name        aphrodite images
// @namespace   https://github.com/murrty/aphrodite
// @version     1.3
// @description e621 image downloader
// @run-at      document-end
// @include     http*://e621.net/posts/*
// @include     http*://*.e621.net/posts/*
// @downloadURL https://github.com/murrty/aphrodite/raw/master/Resources/aphrodite.images.user.js
// @updateURL   https://github.com/murrty/aphrodite/raw/master/Resources/aphrodite.images.user.js
// @grant       none
// ==/UserScript==

// Images
var imageDiv = document.createElement('div');
var imageLink = document.createElement('a');
var imageSettings = document.createElement('a');
var fImagesSidebar = document.getElementById('blacklist-box');
var imageSpacer = document.createElement('br');
var imageSpacerTwo = document.createElement('br');

imageDiv.id = "paginator";

imageLink.id = "download-image";
imageLink.title = "Download this image with aphrodite";
imageLink.href = "images:" + document.URL;
imageLink.appendChild(document.createTextNode('download image'));
imageDiv.appendChild(imageLink);
imageDiv.appendChild(imageSpacer);

imageSettings.id = "image-settings";
imageSettings.href = "images:configuresettings";
imageSettings.title = "Change the Image Downloader settings";
imageSettings.appendChild(document.createTextNode('image settings'));
imageDiv.appendChild(imageSettings);
imageDiv.appendChild(imageSpacerTwo);

//imageDiv.style = "display: normal; text-align: left; padding: 1em 18px 1em;";
imageDiv.className = "status-notice";
fImagesSidebar.parentNode.insertBefore(imageDiv, fImagesSidebar);
