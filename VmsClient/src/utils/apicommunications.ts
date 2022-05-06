import axios from "axios"
import dispatcher from "./dispatcher"

// TODO: use Promises
import * as querystring from "querystring";

export async function deleteUserApiCall(endpoint: any, token: string, onError?: any) {
    const config = {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${token}`,
        }
    };
    return await fetch(`https://localhost:44320/api/${endpoint}`, config);
}

export function doApiCall(endpoint: any, data: any, done: any, quiet = false, onError?: any): Promise<any> {
    const config = {
        method: 'POST',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        body: JSON.stringify({ email: data.email, firstname: data.firstname, lastname: data.lastname, role: data.role, password: data.password, confirmpassword: data.confirmpassword })
    };

    return fetch(`https://localhost:44320/api/${endpoint}`, config)
}

export function doGetApiCall(endpoint: any, barearToken: any, done: any, quiet=false, onError?: any){
    let h = "Bearer " +  barearToken.replace(/['"]+/g, '');
    
    const config = { headers: { 'Authorization': h } , withCredentials : true};
    nodata(endpoint, config, done, quiet, onError);
}

export function doJsonPostApiCall(endpoint: any, barearToken: any, data: any, done: any, quiet=false, onError?: any){
    let h = "Bearer " +  barearToken.replace(/['"]+/g, '');

    const config = { headers: { 'Content-Type': 'application/json', 'Authorization': h } , withCredentials : true};

    post(endpoint, JSON.stringify(data), config, done, quiet, onError);
}

export function doJsonPutApiCall(endpoint: any, barearToken: any, data: any, done: any, quiet=false, onError?: any){
    let h = "Bearer " +  barearToken.replace(/['"]+/g, '');

    const config = { headers: { 'Content-Type': 'application/json', 'Authorization': h } , withCredentials : true};

    put(endpoint, JSON.stringify(data), config, done, quiet, onError);
}

export function doJsonGetApiCall(endpoint: any, done: any, quiet=false, onError?: any){
    const config = { headers: { 'Content-Type': 'application/json' } , withCredentials : true};

    nodata(endpoint, config, done, quiet, onError);
}

export function doJsonDeleteApiCall(endpoint: any, barearToken: any, done: any, quiet = false, onError?: any) {
    let h = "Bearer " +  barearToken.replace(/['"]+/g, '');
    const config = { headers: { 'Content-Type': 'application/json', 'Authorization': h } , withCredentials : true};

    nodata(endpoint, config, done, quiet, onError, axios.delete);
}


export function doApiCallWithSingleFile(endpoint: any, data: any, done: any, quiet = false, onError?: any) {
    let allData = new FormData();
    for (var item in data) {
        if (item !== 'file') {
            allData.append(item, data[item]);
        }
    }
    if (data.file) {
        allData.append('file', data.file, data.file.name);
    }

    const config = {
        headers: { 'content-type': 'multipart/form-data' }
    };

    post(endpoint, allData, config, done, quiet, onError);
}

export function doApiCallWithMultipleFiles(endpoint: any, data: any, done: any, quiet = false, onError?: any) {
    let allData = new FormData();
    let fileList = data.files;

    let fileIndex = 0;
    for (const file of fileList) {
        allData.append("file" + fileIndex, file, file.name);
        fileIndex++;
    }
    allData.append("fileCount", String(fileIndex));

    for (var item in data) {
        if (item != 'files') {
            allData.append(item, data[item]);
        }
    }

    const config = {
        headers: { 'content-type': 'multipart/form-data' }
    };

    post(endpoint, allData, config, done, quiet, onError);
}
function getHost() {
    return "https://localhost:44320";
}

function post(endpoint: any, allData: any, config: any, done: any, quiet: any, onError: any) {
    axios.post(getHost() + "/api/" + endpoint, allData, config)
        .then((result: any) => {
            if (result.status == 200) {
                done(result.data);
            } else {
                if (quiet == false) {
                    dispatcher.dispatch({
                        type: "NOTIFICATION",
                        title: "Oops",
                        message: result.message
                    })
                }
                if (onError !== undefined) {
                    onError();
                }
            }
        })
        .catch((error: any) => {
            if (quiet == false) {
                dispatcher.dispatch({
                    type: "NOTIFICATION",
                    title: "Oops",
                    message: "Could not communicate with server. [" + endpoint + "]"
                })
            }
            if (onError !== undefined) {
                onError(error);
            }
        })
}

function put(endpoint: any, allData: any, config: any, done: any, quiet: any, onError: any){
    axios.put(getHost() + "/api/" + endpoint, allData, config)
        .then((result: any) => {
            if(result.status == 200){
                done(result.data);
            }else{
                if(quiet == false) {
                    dispatcher.dispatch({
                        type: "NOTIFICATION",
                        title: "Oops",
                        message: result.message
                    })
                }
                if(onError !== undefined){
                    onError();
                }
            }
        })
        .catch((error: any) => {
            if(quiet == false){
                dispatcher.dispatch({
                    type: "NOTIFICATION",
                    title: "Oops",
                    message:"Could not communicate with server. [" + endpoint + "]"
                })
            }
            if(onError !== undefined){
                onError(error);
            }
        })
}

function nodata(endpoint: any, config: any, done: any, quiet: any, onError: any, method = axios.get){
    method(getHost() + "/api/" + endpoint, config)
        .then((result: any) => {
            if (result.status == 200) {
                done(result.data);
            } else {
                if (quiet == false) {
                    dispatcher.dispatch({
                        type: "NOTIFICATION",
                        title: "Oops",
                        message: result.message
                    })
                }
                if (onError !== undefined) {
                    onError();
                }
            }
        })
        .catch((error: any) => {
            if (quiet == false) {
                dispatcher.dispatch({
                    type: "NOTIFICATION",
                    title: "Oops",
                    message: "Could not communicate with server. [" + endpoint + "]"
                })
            }
            if (onError !== undefined) {
                onError(error);
            }
        })
}