const _elBody = $("body");
const _elColor = $("#color-ascent");
let _Layout = "";
let _Theme = "";
let _Color = "";
let _CleanBg = "";

export function GenerateNavMenu() {
    initApp.mobileCheckActivation();
    //initApp.buildNavigation($('#js-nav-menu'));
    initApp.listFilter($('#js-nav-menu'), $('#nav_filter_input'), $('#js-primary-nav'));
    SetLayout();
}
export function Layout_Init(layout, theme, color, cleanbg) {
    _Layout = layout;
    _Theme = theme;
    _Color = color;
    _CleanBg = cleanbg;

    SetLayout();
    SetLayoutColor();
    SetLayoutCleanBackground();
    SetLayoutTheme();
}
export function Layout_Change(layout) {
    _Layout = layout;
    localStorage.setItem("SettingLayout", '"' + _Layout + '"');
    SetLayout();
}
export function LayoutColor_Change(color) {
    _Color = color;
    localStorage.setItem("SettingWarna", '"' + _Color + '"');
    console.log(_Color);
    SetLayoutColor();
}
export function LayoutCleanBackground_Change(status) {
    _CleanBg = status;
    localStorage.setItem("SettingCleanBackground", '"' + _CleanBg + '"');
    SetLayoutCleanBackground();
}
export function LayoutTheme_Change(theme) {
    _Theme = theme;
    localStorage.setItem("SettingTema", '"' + _Theme + '"');
    SetLayoutTheme();
}
export function Navigation_Destroy() {
    initApp.destroyNavigation($('#js-nav-menu'));
    initApp.buildNavigation($('#js-nav-menu'));
    initApp.checkNavigationOrientation();
}

function SetLayout() {
    if (_Layout != "Top") {
        $('#js-primary-nav').slimscroll({
            height: '100%',
            color: "#fff",
            size: '4px',
            distance: '4px',
            railOpacity: 0.4,
            wheelStep: 25,
            touchScrollStep: 75
        });
        if (document.getElementById('js-nav-menu-wrapper')) {
            $('#js-nav-menu').menuSlider('destroy');
        }
    }
    else {
        $('#js-primary-nav').slimScroll({ destroy: true });
        $('#js-primary-nav').attr('style', '');
        $('#js-nav-menu').menuSlider({
            element: $('#js-nav-menu'),
            wrapperId: 'js-nav-menu-wrapper'
        });
    }
    switch (_Layout) {
        case "Default":
            _elBody.removeClass("nav-function-top");
            _elBody.removeClass("nav-function-minify");
            _elBody.addClass("nav-function-fixed");
            break;

        case "Minified":
            _elBody.removeClass("nav-function-top");
            _elBody.addClass("nav-function-minify");
            _elBody.addClass("nav-function-fixed");
            break;

        case "Top":
            _elBody.addClass("nav-function-top");
            _elBody.removeClass("nav-function-minify");
            _elBody.removeClass("nav-function-fixed");
            break;
    }
    initApp.buildNavigation($('#js-nav-menu'));
    initApp.checkNavigationOrientation();
}
function SetLayoutColor() {
    if (_Color == "Default") {
        _elColor.attr("href", "");
    }
    else {
        _elColor.attr("href", _Color);
    }
}
function SetLayoutCleanBackground() {
    if (_CleanBg == "Y") {
        _elBody.addClass("mod-clean-page-bg");
    }
    else {
        _elBody.removeClass("mod-clean-page-bg");
    }
}
function SetLayoutTheme() {
    if (_Theme != "Dark") {
        $("#syncfusion-theme").attr("href", "_content/Syncfusion.Blazor.Themes/bootstrap5.css");
    }
    else {
        $("#syncfusion-theme").attr("href", "_content/Syncfusion.Blazor.Themes/bootstrap5-dark.css");
    }

    switch (_Theme) {
        case "Default":
            _elBody.removeClass("mod-skin-light");
            _elBody.removeClass("mod-skin-dark");
            break;
        case "Light":
            _elBody.removeClass("mod-skin-dark");
            _elBody.addClass("mod-skin-light");
            break;
        case "Dark":
            _elBody.removeClass("mod-skin-light");
            _elBody.addClass("mod-skin-dark");
            break;
    }
}
