<template>
    <div>
        <div v-if="products == null" class="loadingGroup">
            <table class="table">
                <thead>
                    <tr>
                        <th><button v-if="user?.admin" type="button" class="btn btn-m btn-primary py-1" title="Add" @click="onAdd()">Add</button></th>
                        <th><span>Name</span><button type="button" class="btn"><i class="gg-arrows-v"></i></button></th>
                        <th><span>ID</span><button type="button" class="btn"><i class="gg-arrows-v"></i></button></th>
                        <th><span>Type</span><button type="button" class="btn"><i class="gg-arrows-v"></i></button></th>
                        <th><span>Quantity</span><button type="button" class="btn"><i class="gg-arrows-v"></i></button></th>
                        <th v-if="user?.admin"><span>Active</span></th>
                        <th v-if="user?.admin"></th>
                    </tr>
                </thead>

                <tbody>
                    <tr v-for="n in 10" :key="n">
                        <td><div class="loadingPicture loadingAnimate"></div></td>
                        <td><div class="loadingText loadingAnimate" style="width: 10em"></div></td>
                        <td><div class="loadingText loadingAnimate" style="width: 2em"></div></td>
                        <td><div class="loadingText loadingAnimate" style="width: 10em"></div></td>
                        <td><div class="loadingText loadingAnimate" style="width: 2em"></div></td>
                        <td v-if="user?.admin"><div class="loadingText loadingAnimate" style="width: 2em"></div></td>
                        <td v-if="user?.admin"><div class="loadingPicture loadingAnimate"></div></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <table v-else class="table">
            <thead>
                <tr>
                    <th><button v-if="user?.admin" type="button" class="btn btn-m btn-primary py-1" title="Add" @click="onAdd()">Add</button></th>
                    <th><span>Name</span><button type="button" class="btn" @click="onSortClicked('name')"><i class="gg-arrows-v"></i></button></th>
                    <th style="min-width: 70px"><span>ID</span><button type="button" class="btn" @click="onSortClicked('id')"><i class="gg-arrows-v"></i></button></th>
                    <th style="min-width: 90px"><span>Type</span><button type="button" class="btn" @click="onSortClicked('type')"><i class="gg-arrows-v"></i></button></th>
                    <th style="min-width: 120px"><span>Quantity</span><button type="button" class="btn" @click="onSortClicked('quantity')"><i class="gg-arrows-v"></i></button></th>
                    <th v-if="user?.admin"><span>Active</span></th>
                    <th v-if="user?.admin"></th>
                </tr>
            </thead>

            <tbody>
                <tr v-for="product in products" :key="product.id">
                    <td v-if="product.imageLink" class="clickable" @click="onProduct(product.id)"><img v-bind:src="'/products/' + product.imageLink" height="70" class="d-inline-block"></td>
                    <td v-else class="clickable" @click="onProduct(product.id)"><img src="/products/na.png" height="70" class="d-inline-block"></td>
                    <td class="text-start clickable" @click="onProduct(product.id)">{{product.name}}</td>
                    <td class="clickable" @click="onProduct(product.id)">{{product.id}}</td>
                    <td class="clickable" @click="onProduct(product.id)">{{product.type}}</td>
                    <td class="clickable" @click="onProduct(product.id)">{{product.quantity}}</td>
                    <td v-if="user?.admin" class="clickable" @click="onProduct(product.id)">{{product.active === true ? "yes" : "no"}}</td>
                    <td v-if="user?.admin">
                        <button v-if="product.active === true" type="button" class="btn" title="Deactivate" @click="onActivation(product.id, false)"><i class="gg-close-r deactivate-button"></i></button>
                        <button v-else type="button" class="btn" title="Activate" @click="onActivation(product.id, true)"><i class="gg-check-r activate-button"></i></button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
export default {
    name: "ProductList",
    props: {
        products: Array
    },
    methods: {
        onActivation(id, activate) {this.$emit('onActivation', id, activate);},
        onAdd() {this.$emit('onAdd');},
        onSortClicked(column) {this.$emit('onSort', column);},
        onProduct(id) {this.$router.push({ name: 'ProductDetails', params: { id } });}
    },
    computed: {
        user() {
            return this.$store.state.user;
        }
    },
    emits: ['onActivation', 'onProduct', 'onSort','onAdd']
};
</script>

<style scoped lang="scss">
    @import "../../node_modules/css.gg/icons/scss/arrows-v.scss";
    @import "../../node_modules/css.gg/icons/scss/close-r.scss";
    @import "../../node_modules/css.gg/icons/scss/check-r.scss";
    @import "../styles/loading.scss";

    .deactivate-button {
        background-color: orangered;
    }

    .activate-button {
        background-color: greenyellow;
    }

    .clickable {
        cursor: pointer;
    }

    .searchBox {
        display: block;
    }
</style>