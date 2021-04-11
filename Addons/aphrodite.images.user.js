// ==UserScript==
// @id          aphrodite-img
// @name        aphrodite images
// @description e621 image downloader
// @namespace   murrty.aphrodite
// @version     1.6

// @homepage    https://github.com/murrty/aphrodite
// @updateURL   https://github.com/murrty/aphrodite/raw/master/Resources/aphrodite.images.user.js
// @downloadURL https://github.com/murrty/aphrodite/raw/master/Resources/aphrodite.images.user.js

// @run-at      document-end
// @grant       none

// @include     http*://e621.net/posts/*
// ==/UserScript==

// Images
var imageDiv = document.createElement('div');
var tagsLinkImage = document.createElement('a');
var imageLink = document.createElement('a');
var imageSettings = document.createElement('a');
var fImagesSidebar = document.getElementById('blacklist-box');
var imageSpacer = document.createElement('br');
var imageSpacerTwo = document.createElement('br');
var imageSpacerThree = document.createElement('br');

imageDiv.id = "paginator";

if (document.URL.indexOf("?q=") > -1) {
    tagsLinkImage.id = "download-tags-image";
    tagsLinkImage.title = "Download the searched tag(s)"
    var foundTags = document.getElementsByName('tags')[0].value;
    tagsLinkImage.href = "tags:" + foundTags.replace(new RegExp(" ", "g"), '|');
    tagsLinkImage.appendChild(document.createTextNode('download tag(s)'));
    imageDiv.appendChild(tagsLinkImage);
    imageDiv.appendChild(imageSpacer);
}

imageLink.id = "download-image";
imageLink.title = "Download this image with aphrodite";
imageLink.href = "images:" + document.URL;
imageLink.appendChild(document.createTextNode('download image'));
imageDiv.appendChild(imageLink);
imageDiv.appendChild(imageSpacerTwo);

imageSettings.id = "image-settings";
imageSettings.href = "images:configuresettings";
imageSettings.title = "Change the Image Downloader settings";
imageSettings.appendChild(document.createTextNode('image settings'));
imageDiv.appendChild(imageSettings);

imageDiv.className = "status-notice";
fImagesSidebar.parentNode.insertBefore(imageDiv, fImagesSidebar);
fImagesSidebar.parentNode.insertBefore(imageSpacerThree, fImagesSidebar);