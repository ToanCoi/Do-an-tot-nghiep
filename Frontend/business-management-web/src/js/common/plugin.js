import Enumeration from "./Enumeration";
import Resource from "./Resource";
import CommonFn from "./CommonFn";

export function pluginInstall(app) {
    app.CommonFn = CommonFn;
    app.Resource = Resource;
    app.Enumeration = Enumeration;
}