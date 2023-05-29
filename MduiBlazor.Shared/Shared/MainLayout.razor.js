export function AddWindowWidthListener(objReference) {
    let eventListener = () => UpdateLayout(objReference);
    window.addEventListener("resize", eventListener);
}

export function RemoveWindowWidthListener() {
    window.removeEventListener("resize");
}

export function AddScrollListener(objReference) {
    let eventListener = () => UpdateScrollToTop(objReference);
    window.addEventListener('scroll', eventListener);
}

export function RemoveScrollListener() {
    window.removeEventListener("scroll");
}

function UpdateLayout(objReference) {
    objReference.invokeMethodAsync("UpdateLayout", window.innerWidth);
}

function UpdateScrollToTop(objReference) {
    let scrollTop = GetScrollTop();
    objReference.invokeMethodAsync("UpdateScrollToTop", scrollTop);
}

export function ScrollToTop() {
    document.body.scrollIntoView({ behavior: "smooth" });
}

export function GetWindowWidth() {
    return window.innerWidth;
}

export function GetScrollTop() {
    return window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0;
}

export function ScrollToElementById(elementId) {
    var element = document.getElementById(elementId);
    if (!element)
        return false;

    element.scrollIntoView({ behavior: "smooth", block: "center" });
    return true;
}