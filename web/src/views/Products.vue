<template>
    <global-header />

    <div class="products row flex-grow-1 align-content-start">
        <h1 class="pt-5">Products</h1>
        <SearchBox @search="onSearch" placeholder="Search products" />
        <ProductList @on-Activation="onActivation" @on-add="onAdd" @on-sort="onSort" @on-page="onPage" :products="productsList"/>
        <div class="d-flex justify-content-end">
            <a v-if="productsQuery && productsQuery.offset > 0" class="p-2" href="" @click.prevent="onPage('previous')">Previous</a>
            <a v-if="productsQuery && productsQuery.offset + productsQuery.products.length < productsQuery.total" class="p-2" href="" @click.prevent="onPage('next')">Next</a>
        </div>
    </div>

    <AddProductModal v-if="showAddProduct" @on-Close="onAddProductModalClose" />
    <ConfirmationBox v-if="confirmBox" :text="confirmBox" @on-Close="onActivationConfirmation" />

    <global-footer />
</template>

<script>
// @ is an alias to /src
import GlobalHeader from "@/components/GlobalHeader.vue";
import GlobalFooter from "@/components/GlobalFooter.vue";
import ProductList from "@/components/ProductList.vue";
import SearchBox from "@/components/SearchBox.vue";
import { useToast } from "vue-toastification";
import CorpAPI from "@/api";

import AddProductModal from "@/components/AddProductModal.vue";
import ConfirmationBox from "@/components/ConfirmationBox.vue";

export default {
    name: "Products",
    components: {
        GlobalHeader,
        GlobalFooter,
        ProductList,
        SearchBox,
        ConfirmationBox,
        AddProductModal,
    },
    data() {
        return {
            productsQuery: null,
            productsList: null,
            showAddProduct: false,
            confirmBox: null,
            searchTerm: "",
            sortColumn: "id",
            sortDirection: "asc",
        };
    },
    setup() {
        const toast = useToast();
        return { toast };
    },
    async mounted() {
        await this.loadProducts(0, 10);
    },
    methods: {
        async loadProducts(offset, size) {
            let data = this;

            this.productsQuery = null;
            this.productsList = null;

            let filter = [];

            if (this.searchTerm && this.searchTerm.length > 0)
                filter.push(this.searchTerm);

            if(!this.$store.state.user?.admin)
                filter.push("hideinactive");

            let apiProducts = await CorpAPI.getProducts(offset, size, filter, this.sortColumn + "_" + this.sortDirection);
            
            if(apiProducts) {
                setTimeout(function () {
                    data.productsQuery = apiProducts;
                    data.productsList = apiProducts.products;
                }, 1000);
            }
            else {
                data.toast.error("Loading products failed", { timeout: false });
            }
        },
        async onActivation(id, active) {
            this.confirmBox = {
                title: active ? "Confirm Activate" : "Confirm Deactivate",
                message: `Are you sure you want to ${active ? 'activate' : 'deactivate'} product ${id}?`,
                cancel: "No",
                accept: "Yes",
                id: id,
                active: active,
            };
        },
        async onActivationConfirmation(accepted) {
            if(accepted) {
                const id = this.confirmBox.id;
                const active = this.confirmBox.active;
                this.confirmBox = null;

                let product = this.productsList.find(p => p.id === id);
                product.active = active

                if(await CorpAPI.updateProduct(product)) {
                    this.toast.success(`Product ${id} ${active ? 'activated' : 'deactivated'}`);
                }
                else {
                    this.toast.error(`Product ${active ? 'activation' : 'deactivation'} failed`, { timeout: false });
                }
            }
            else {
                this.confirmBox = null;
            }
        },
        async onAdd() {
            this.showAddProduct = true;
        },
        async onAddProductModalClose(success) {
            this.showAddProduct = false;

            if(success) {
                await this.loadProducts(this.productsQuery.offset, 10);
            }
        },
        async onSearch(term) {
            this.searchTerm = term;
            this.sortColumn = "id";
            this.sortDirection = "asc";
            await this.loadProducts(0, 10);
        },
        async onSort(column) {
            if(column === this.sortColumn) {
                if(this.sortDirection === "asc")
                    this.sortDirection = "dec";
                else
                    this.sortDirection = "asc";
            }
            else {
                this.sortColumn = column;

                if(this.sortColumn === "quantity")
                    this.sortDirection = "dec";
                else
                    this.sortDirection = "asc";
            }

            await this.loadProducts(0, 10);
        },
        async onPage(direction) {
            if(direction === "next") {
                await this.loadProducts(this.productsQuery.offset + this.productsQuery.products.length, 10);
            }
            else if(direction === "previous") {
                if(this.productsQuery.offset - 10 >= 0)
                    await this.loadProducts(this.productsQuery.offset - 10, 10);
                else
                    await this.loadProducts(0, 10);
            }
        }
    },
    computed: {
        user() {
            return this.$store.state.user;
        }
    },
    watch: {
        async user() {
            await this.loadProducts(0, 10);
        }
    }
};
</script>

<style scoped lang="scss">

</style>