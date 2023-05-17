let windowEventListeners = {};

export function AddWindowWidthListener(objReference) {
    let eventListener = () => UpdateLayout(objReference);
    window.addEventListener("resize", eventListener);
    windowEventListeners[objReference] = eventListener;
}

export function RemoveWindowWidthListener(objReference) {
    window.removeEventListener("resize", windowEventListeners[objReference]);
}

function UpdateLayout(objReference) {
    objReference.invokeMethodAsync("UpdateLayout", window.innerWidth);
}

export function GetWindowWidth() {
    return window.innerWidth;
}

export function ScrollToElementById(elementId) {
    var element = document.getElementById(elementId);
    if (!element)
        return false;

    element.scrollIntoView({ behavior: "smooth", block: "center" });
    return true;
}