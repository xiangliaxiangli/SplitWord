<template>
  <div>
    <div class="line"></div>
    <el-menu
        router
        :default-active="activeIndex"
        class="el-menu-demo"
        mode="horizontal"
        @select="handleSelect"
        background-color="#545c64"
        text-color="#fff"
        active-text-color="#ffd04b"
        >
      <el-menu-item  index="/">知识上传</el-menu-item>
      <el-menu-item  index="/Search">知识查询</el-menu-item>
      <el-menu-item>退出</el-menu-item>
    </el-menu>
  </div>
</template>

<script>
export default {
  data() {
    return {
      activeIndex: '/',
      activeIndex2: '/'
    };
  },
  methods: {
    handleSelect(key, keyPath) {
      console.log(key, keyPath);
    }
  }
}
</script>