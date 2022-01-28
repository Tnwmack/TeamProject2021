import axios from "axios";

let baseAPI;

if(process?.env?.NODE_ENV === 'development') {
    baseAPI = "http://localhost:11959/api/";
}
else {
    baseAPI = `${window.location.protocol}//${window.location.host}/api/`;
}

export default {
    async getListGeneric(apiurl, offset, size, filter, order) {
        try {
            let url = new URL(baseAPI + apiurl);

            if (offset)
                url.searchParams.append("offset", offset);

            if (size)
                url.searchParams.append("size", size);

            if(filter)
            {
                for(let i = 0; i < filter.length; i ++)
                {
                    url.searchParams.append(`filter[${i}]`, filter[i]);
                }
            }

            if(order)
                url.searchParams.append("order", order);

            const response = await axios.get(url.href);            
            return response.data;
        }

        catch(error) {
            return null;
        }
    },
    async getItemGeneric(apiurl, id) {
        try {
            let url = new URL(baseAPI + `${apiurl}/${id}`);
            const response = await axios.get(url.href);            
            return response.data;
        }

        catch(error) {
            return null;
        }
    },
    async getProducts(offset, size, filter, order) {
        return this.getListGeneric("products", offset, size, filter, order);
    },
    async getProduct(id) {
        return this.getItemGeneric("products", id);
    },
    async getEmployees(offset, size, filter, order) {
        return this.getListGeneric("employees", offset, size, filter, order);
    },
    async getEmployee(id) {
        return this.getItemGeneric("employees", id);
    },
    async createGeneric(apiurl, data) {
        try {
            await axios.post(baseAPI + `${apiurl}`, data);
            return true;
        }

        catch(error) {
            return false;
        }
    },
    async uploadProductImage(data) {
        try {
            const response = await axios.post(baseAPI + "products/image", data);
            return response.data;
        }

        catch(error) {
            return null;
        }
    },
    async createProduct(product) {
        return this.createGeneric("products", product);
    },
    async createEmployee(employee) {
        return this.createGeneric("employees", employee);
    },
    async updateGeneric(apiurl, data) {
        let id = data.id;

        try {
            await axios.put(baseAPI + `${apiurl}/${id}`, data);
            return true;
        }

        catch(error) {
            return false;
        }
    },
    async updateProduct(product) {
        return this.updateGeneric("products", product);
    },
    async updateEmployee(employee) {
        return this.updateGeneric("employees", employee);
    },
};