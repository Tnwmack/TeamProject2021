<template>
    <global-header />

    <h1 class="pt-5">Product Details</h1>
    <div>
        <img v-if="product?.imageLink" v-bind:src="'/products/' + product.imageLink" height="300" class="d-inline-block">
        <img v-else src='/products/na.png' height="100" class="d-inline-block">
    </div>

    <div class="about row flex-grow-1 align-content-left">
        <div>Name: {{product?.name}}</div>
        <div>Id: {{product?.id}}</div>
        <div>Product Type: {{product?.type}}</div>
        <div>Quantity: {{product?.quantity}}</div>
        <div>Description: {{product?.description}}</div>
    </div>

    <th><button v-if="user?.admin" type="button" class="btn btn-m btn-primary py-1" title="Edit" @click="onEdit()">Edit</button></th>

    <AddProductModal v-if="showEditProduct" @on-Close="onAddProductModalClose" :product="product" />

    <global-footer />
</template>

<script>
import GlobalHeader from '@/components/GlobalHeader.vue'
import GlobalFooter from '@/components/GlobalFooter.vue'
import { useToast } from "vue-toastification";
import CorpAPI from "@/api";

import AddProductModal from "@/components/AddProductModal.vue";

    export default {
        name: 'ProductDetails',
        components: {
            GlobalHeader,
            GlobalFooter,
            AddProductModal
        },
        data() {
            return {
                showEditProduct: false,
                product: null
            };
        },
        setup() {
            const toast = useToast();
            return { toast };
        },
        computed: {
            user() {
                return this.$store.state.user;
            }
        },
        async mounted() {
            await this.loadDetails(this.$route.params.id);
        },
        methods: {
            async loadDetails(id) {
                this.product = await CorpAPI.getProduct(id);

                if (this.product) {
                    if (!this.product.active && !this.user?.admin) {
                        this.toast.error("Selected product is no longer active.", { timeout: false });
                    }
                }
                else {
                    this.toast.error("Loading product failed", { timeout: false });
                }
            },
            onEdit() {
                this.showEditProduct = true;
            },
            async onAddProductModalClose(success) {
                this.showEditProduct = false;

                if(success)
                    await this.loadDetails(this.$route.params.id);
            }
        }
    }
</script>