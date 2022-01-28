<template>
    <form @submit.prevent="onSubmit" class="search-box">
        <input v-model="searchTerm" type="text" class="form-control" :placeholder="placeholder">
        <span v-show="showClear" class="gg-close-o clear-search" @click="onClear"></span>
    </form>
</template>

<script>
export default {
    name: "SearchBox",
    props: {
        placeholder: String
    },
    data() {
        return {
            searchTerm: "",
        };
    },
    methods: {
        onSubmit() { this.$emit('search', this.searchTerm); },
        onClear() {
            this.searchTerm = "";
            this.$emit('search', this.searchTerm);
        }
    },
    computed: {
        showClear() {
            return this.searchTerm.length > 0;
        }
    },
    emits: ['search']
};
</script>

<style scoped lang="scss">
    @import "../../node_modules/css.gg/icons/scss/close-o.scss";

    .search-box {
        position: relative;
    }

    .clear-search {
        position: absolute;
        width: 22px;
        right: 20px;
        top: 8px;
        cursor: pointer;
        color: grey;
    }

</style>